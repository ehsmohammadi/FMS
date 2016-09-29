using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Castle.DynamicProxy.Generators.Emitters.CodeBuilders;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class UploaderVM : WorkspaceViewModel
    {

        #region Prop
        public const string UPLOAD_DIALOG_FILTER = "All files (*.*)|*.*|PDF File (*.pdf)|*.pdf";
        //public const int CHUNK_SIZE = 4096;

        private OpenFileDialog dlg;
        private IFuelController _fuelController;
        private IFileServiceWrapper _fileServiceWrapper;
        public AttachmentType AttachmentType { get; set; }

        private ObservableCollection<AttachmentDto> _attachmentDtos;
        public ObservableCollection<AttachmentDto> AttachmentDtos
        {
            get { return _attachmentDtos; }
            set
            {
                this.SetField(p => p.AttachmentDtos, ref _attachmentDtos, value);

            }
        }
        private AttachmentDto _attachmentDto;
        public AttachmentDto AttachmentDto
        {
            get { return _attachmentDto; }
            set
            {
                this.SetField(p => p.AttachmentDto, ref _attachmentDto, value);

            }
        }

        public long EntityId { get; set; }

        private Stream _dataStream;
        private long _bytesTotal;
        private long _bytesUploaded;
        private string _fileName;
        long progressBarMaximum;
        long progressBarValue;
        private bool isSuccess;
        private Guid _guid;
        private string ext;
        private bool _isVisible = false;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                this.SetField(p => p.IsVisible, ref _isVisible, value);

            }
        }
        public long ProgressBarMaximum
        {
            get { return progressBarMaximum; }
            set { this.SetField(p => p.ProgressBarMaximum, ref progressBarMaximum, value); }
        }
        public string FileName
        {
            get { return _fileName; }
            set
            {
                this.SetField(p => p.FileName, ref _fileName, value);

            }
        }

        public long ProgressBarValue
        {
            get { return progressBarValue; }
            set
            {
                this.SetField(p => p.ProgressBarValue, ref progressBarValue, value);
            }
        }


        private CommandViewModel selectCommand;
        public CommandViewModel SelectCommand
        {
            get
            {
                selectCommand = new CommandViewModel("انتخاب فایل", new DelegateCommand(() =>
                {

                    var retval = dlg.ShowDialog();

                    if (retval != null && retval == true)
                    {
                        ResetValueUploader();
                        _dataStream = dlg.File.OpenRead();
                        isSuccess = false;
                        _bytesTotal = _dataStream.Length;
                        _bytesUploaded = 0;
                        FileName = dlg.File.Name.Split('.')[0];
                        ext = dlg.File.Name.Split('.')[1];
                        //ProgressBarMaximum = _bytesTotal;
                        //ProgressBarValue = 0;

                    }
                    else
                    {
                        _fuelController.ShowMessage("لطفا فایل مورد نظر را انتخاب نمایید");
                    }

                }));
                return selectCommand;
            }

        }



        private CommandViewModel uploadCommand;
        public CommandViewModel UploadCommand
        {
            get
            {
                ShowBusyIndicator("درحال ارسال اطلاعات ....");

                uploadCommand = new CommandViewModel("ارسال فایل", new DelegateCommand(() =>
                {
                    Uploade();
                }));

               
                return uploadCommand;
            }

        }


        private void Uploade()
        {
          //  AutoResetEvent syncEvent = new AutoResetEvent(false);


            if (_dataStream.Length < 2000000)
            {

                _fileServiceWrapper.Upload((res) => _fuelController.BeginInvokeOnDispatcher(() =>
                {
                  //  syncEvent.Set();
                    HideBusyIndicator();
                    _fuelController.ShowMessage(res ? " فایل با موفقیت ارسال گردیده شد" : "خطا در ارسال فایل");
                   
                   
                }
                  ), _dataStream, FileName, ext, EntityId, AttachmentType, _bytesTotal);

               


            }
            else
            {
                _fuelController.ShowMessage(" فایل مورد نظر نباید بیشتر از 2 مگابیت باشد");
            }

          //  syncEvent.WaitOne();
         //   Load();

        }

        private CommandViewModel refreshCommand;
        public CommandViewModel RefreshCommand
        {
            get
            {
                refreshCommand = new CommandViewModel("بروز رسانی لیست", new DelegateCommand(() =>Load()));
 
                return refreshCommand;
            }

        }


        private CommandViewModel deleteCommand;
        public CommandViewModel DeleteCommand
        {
            get
            {
                deleteCommand = new CommandViewModel("حذف فایل پیوست ", new DelegateCommand(() =>
                {

                    if (AttachmentDto!=null && AttachmentDto.Id != 0)
                    {
                        _fileServiceWrapper.Delete((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                        {

                            HideBusyIndicator();
                            if (exp == null)
                            {
                                Load();
                            }
                            else
                            {
                                _fuelController.HandleException(exp);
                            }


                        }), AttachmentDto.Id);
                    }
                    else
                    {
                        _fuelController.ShowMessage("لطفا فایل پیوست مورد نظر را انتخاب نمایید");
                    }


                }));
                return deleteCommand;
            }

        }

        private CommandViewModel _submitCommand;
        public CommandViewModel SubmitCommand
        {
            get
            {
                _submitCommand = new CommandViewModel("افزودن فایل", new DelegateCommand(() =>
                {

                    ResetValueUploader();

                }));
                return _submitCommand;
            }

        }
        #endregion


        public UploaderVM(IFuelController fuelController, IFileServiceWrapper fileServiceWrapper)
        {
            _fuelController = fuelController;
            _fileServiceWrapper = fileServiceWrapper;
            IsVisible = true;
            dlg = new OpenFileDialog();
            dlg.Filter = UPLOAD_DIALOG_FILTER;
        }


        //private void UploadFileChunk()
        //{
        //    _guid = Guid.NewGuid();

        //    string uploadUri = ApiConfig.HostAddress + "FileUpload.ashx?append={0}&filename={1}&ext={2}";
        //    if (_bytesUploaded == 0)
        //    {
        //        uploadUri = String.Format(uploadUri, 0, _guid.ToString(),ext); // Dont't append
        //    }
        //    else if (_bytesUploaded < _bytesTotal)
        //    {
        //        uploadUri = String.Format(uploadUri, 1, _guid.ToString(), ext); // append
        //    }
        //    else
        //    {
        //        return;  // Upload finished
        //    }

        //    //  byte[] fileContent = new byte[CHUNK_SIZE];
        //    byte[] fileContent = new byte[_bytesTotal];
        //    int bytesRead = _dataStream.Read(fileContent, 0, Int32.Parse(_bytesTotal.ToString()));
        //    _dataStream.Flush();


        //    WebClient wc = new WebClient();

        //    wc.OpenWriteCompleted += new OpenWriteCompletedEventHandler(wc_OpenWriteCompleted);
        //    Uri u = new Uri(uploadUri);

        //    wc.OpenWriteAsync(u, null, new object[] { fileContent, bytesRead });

        //    _bytesUploaded += fileContent.Length;


        //}

        //void wc_OpenWriteCompleted(object sender, OpenWriteCompletedEventArgs e)
        //{
        //    ProgressBarValue = _bytesUploaded;

        //    _fuelController.BeginInvokeOnDispatcher(() =>
        //             {
        //                 if (e.Error == null)
        //                 {

        //                     object[] objArr = e.UserState as object[];
        //                     byte[] fileContent = objArr[0] as byte[];
        //                     int bytesRead = Convert.ToInt32(objArr[1]);
        //                     Stream outputStream = e.Result;
        //                     outputStream.Write(fileContent, 0, bytesRead);
        //                     outputStream.Close();
        //                     isSuccess = true;

        //                     _fuelController.ShowConfirmationBox("فایل با موفقیت ارسال گردیده شد", "");

        //                 }
        //             });

        //}

        public void Load()
        {
            _fileServiceWrapper.Get((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
            {
                if (exp != null)
                {
                    _fuelController.HandleException(exp);
                }
                else
                {
                    AttachmentDtos = new ObservableCollection<AttachmentDto>();
                    res.ForEach(c =>
                    {
                        if (c.Ext.ToLower() == "pdf")
                            c.Ext = "../../Assets/pdf.png";
                        else if (c.Ext.ToLower() == "doc" || c.Ext.ToLower() == "docx")
                            c.Ext = "../../Assets/word.png";
                        else if (c.Ext.ToLower() == "xls" || c.Ext.ToLower() == "xlsx")
                            c.Ext = "../../Assets/excel.png";
                        else if (c.Ext.ToLower() == "jpg" || c.Ext.ToLower() == "png" || c.Ext.ToLower() == "gif" || c.Ext.ToLower() == "bmp")
                            c.Ext = "../../Assets/image.png";
                        else
                            c.Ext = "../../Assets/text.png";
                        AttachmentDtos.Add(c);
                    });
                }

            }), EntityId, AttachmentType);
        }

        public void Visible()
        {
            IsVisible = true;
        }
        public void InVisible()
        {
            IsVisible = false;
        }
        void ResetValueUploader()
        {
            _dataStream = null;
            _bytesTotal = 0;
            _bytesUploaded = 0;
            _fileName = "";
            isSuccess = false;
            progressBarMaximum = 0;
            progressBarValue = 0;
            _guid = Guid.Empty;
            ext = "";
        }

    }
}
