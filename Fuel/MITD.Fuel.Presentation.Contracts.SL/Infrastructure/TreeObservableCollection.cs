using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Fuel.Presentation.Contracts.SL.Extensions;

namespace MITD.Fuel.Presentation.Contracts.SL.Infrastructure
{
    public interface ITreeNode : INotifyPropertyChanged
    {
        bool IsSelected { get; set; }
        bool IsExpanded { get; set; }

        ITreeNode Parent { get; set; }
        IList<ITreeNode> Children { get; }

        void AddChild(ITreeNode child);
        void RemoveChild(ITreeNode child);

    }

    public class TreeObservableCollection<T> : ObservableCollection<T> where T : class, ITreeNode, IDisposable
    {
        #region ctor

        public TreeObservableCollection()
            : base()
        {
            this.SelectedItems = new List<T>();
            this.CollectionChanged += this.OnCollectionChanged;
           
        }

        #endregion

        #region props

        public List<T> SelectedItems { get; private set; }
        #endregion

        #region methods
        private void OnCollectionChanged(object sender ,NotifyCollectionChangedEventArgs e  )
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                this.OnAdd(e.NewItems);
            else if (e.Action == NotifyCollectionChangedAction.Remove)
                this.OnDelete(e.OldItems);
            else if (e.Action == NotifyCollectionChangedAction.Reset)
                this.OnReset();
        }

        private void AttachPropertyChanged(T ent)
        {
            ent.PropertyChanged += (s, e) =>
                                       {   
                                           if (e.PropertyName == ent.GetPropertyName(x => x.IsSelected))
                                           {
                                               if (ent.IsSelected)
                                                   if (!this.SelectedItems.Contains(ent))
                                                       this.SelectedItems.Add(ent);
                                                   else if (!ent.IsSelected)
                                                       this.SelectedItems.Remove(ent);
                                           }

                                       };

        }


        private void OnAdd(IList entities)
        {
            foreach (var entity in entities.Cast<T>())
            {
                this.AttachPropertyChanged(entity);
                if (entity.Children != null)
                    this.OnAdd(entity.Children as IList);
            }
        }

        private void OnDelete(IList entities)
        {
            foreach (var entity in entities.Cast<T>())
            {
                this.SelectedItems.Remove(entity);
            }
        }
        private void OnReset()
        {
            this.SelectedItems.RemoveAll((a) => true);
        }

        public void Dispose()
        {
            this.CollectionChanged -= this.OnCollectionChanged;
            this.SelectedItems.RemoveAll((x)=>true);
            this.SelectedItems = null;
        }

        public void AddRange(IEnumerable<T> nodes)
        {
            foreach (var node in nodes)
            {
                this.Add(node);

            }
        }

        #endregion

    }
}
