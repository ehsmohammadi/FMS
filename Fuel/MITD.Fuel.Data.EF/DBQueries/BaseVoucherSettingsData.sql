﻿SET IDENTITY_INSERT [Fuel].[Accounts] ON
INSERT INTO [Fuel].[Accounts] ([Id], [Name], [Code]) VALUES (1, N'موجودی کالا - سوخت سبک', N'15101')
INSERT INTO [Fuel].[Accounts] ([Id], [Name], [Code]) VALUES (2, N'موجودی کالا - سوخت سنگین', N'15111')
INSERT INTO [Fuel].[Accounts] ([Id], [Name], [Code]) VALUES (3, N'مالکان کشتی های استیجاری', N'31215')
INSERT INTO [Fuel].[Accounts] ([Id], [Name], [Code]) VALUES (5, N'تدارک کنندگان کشتی ها', N'31118')
INSERT INTO [Fuel].[Accounts] ([Id], [Name], [Code]) VALUES (6, N'هزینه مصرف سوخت سبک', N'71402')
INSERT INTO [Fuel].[Accounts] ([Id], [Name], [Code]) VALUES (7, N'هزینه مصرف سوخت سنگین', N'71412')
INSERT INTO [Fuel].[Accounts] ([Id], [Name], [Code]) VALUES (9, N'بدهکاران اشخاص', N'14205')
INSERT INTO [Fuel].[Accounts] ([Id], [Name], [Code]) VALUES (10, N'اجاره کنندگان کشتی ها', N'13355')
SET IDENTITY_INSERT [Fuel].[Accounts] OFF

SET IDENTITY_INSERT [Fuel].[VoucherSetings] ON
INSERT INTO [Fuel].[VoucherSetings] ([Id], [VoucherMainRefDescription], [VoucherMainDescription], [CompanyId], [VoucherDetailTypeId], [VoucherTypeId]) VALUES (2, N'x', N'x', 3, 6, 1)
INSERT INTO [Fuel].[VoucherSetings] ([Id], [VoucherMainRefDescription], [VoucherMainDescription], [CompanyId], [VoucherDetailTypeId], [VoucherTypeId]) VALUES (3, N'x', N'x', 3, 1, 2)
INSERT INTO [Fuel].[VoucherSetings] ([Id], [VoucherMainRefDescription], [VoucherMainDescription], [CompanyId], [VoucherDetailTypeId], [VoucherTypeId]) VALUES (4, N'x', N'x', 3, 6, 1)
INSERT INTO [Fuel].[VoucherSetings] ([Id], [VoucherMainRefDescription], [VoucherMainDescription], [CompanyId], [VoucherDetailTypeId], [VoucherTypeId]) VALUES (5, N'x', N'x', 3, 3, 3)
INSERT INTO [Fuel].[VoucherSetings] ([Id], [VoucherMainRefDescription], [VoucherMainDescription], [CompanyId], [VoucherDetailTypeId], [VoucherTypeId]) VALUES (8, N'x', N'x', 3, 9, 5)
INSERT INTO [Fuel].[VoucherSetings] ([Id], [VoucherMainRefDescription], [VoucherMainDescription], [CompanyId], [VoucherDetailTypeId], [VoucherTypeId]) VALUES (9, N'x', N'x', 1, 2, 3)
INSERT INTO [Fuel].[VoucherSetings] ([Id], [VoucherMainRefDescription], [VoucherMainDescription], [CompanyId], [VoucherDetailTypeId], [VoucherTypeId]) VALUES (10, N'x', N'x', 3, 7, 3)
SET IDENTITY_INSERT [Fuel].[VoucherSetings] OFF


SET IDENTITY_INSERT [Fuel].[VoucherSetingDetails] ON
INSERT INTO [Fuel].[VoucherSetingDetails] ([Id], [VoucherSetingId], [VoucherCeditRefDescription], [VoucherDebitDescription], [VoucherDebitRefDescription], [VoucherCreditDescription], [GoodId]) VALUES (3, 2, N'', N'', N'', N'', 3001)
INSERT INTO [Fuel].[VoucherSetingDetails] ([Id], [VoucherSetingId], [VoucherCeditRefDescription], [VoucherDebitDescription], [VoucherDebitRefDescription], [VoucherCreditDescription], [GoodId]) VALUES (4, 3, N'', N'', N'', N'', 3001)
INSERT INTO [Fuel].[VoucherSetingDetails] ([Id], [VoucherSetingId], [VoucherCeditRefDescription], [VoucherDebitDescription], [VoucherDebitRefDescription], [VoucherCreditDescription], [GoodId]) VALUES (5, 3, N'', N'', N'', N'', 3002)
INSERT INTO [Fuel].[VoucherSetingDetails] ([Id], [VoucherSetingId], [VoucherCeditRefDescription], [VoucherDebitDescription], [VoucherDebitRefDescription], [VoucherCreditDescription], [GoodId]) VALUES (6, 2, N'', N'', N'', N'', 3002)
INSERT INTO [Fuel].[VoucherSetingDetails] ([Id], [VoucherSetingId], [VoucherCeditRefDescription], [VoucherDebitDescription], [VoucherDebitRefDescription], [VoucherCreditDescription], [GoodId]) VALUES (7, 5, N'', N'', N'', N'', 3002)
INSERT INTO [Fuel].[VoucherSetingDetails] ([Id], [VoucherSetingId], [VoucherCeditRefDescription], [VoucherDebitDescription], [VoucherDebitRefDescription], [VoucherCreditDescription], [GoodId]) VALUES (8, 5, N'', N'', N'', N'', 3001)
INSERT INTO [Fuel].[VoucherSetingDetails] ([Id], [VoucherSetingId], [VoucherCeditRefDescription], [VoucherDebitDescription], [VoucherDebitRefDescription], [VoucherCreditDescription], [GoodId]) VALUES (13, 8, N'', N'', N'', N'', 3001)
INSERT INTO [Fuel].[VoucherSetingDetails] ([Id], [VoucherSetingId], [VoucherCeditRefDescription], [VoucherDebitDescription], [VoucherDebitRefDescription], [VoucherCreditDescription], [GoodId]) VALUES (14, 8, N'', N'', N'', N'', 3002)
INSERT INTO [Fuel].[VoucherSetingDetails] ([Id], [VoucherSetingId], [VoucherCeditRefDescription], [VoucherDebitDescription], [VoucherDebitRefDescription], [VoucherCreditDescription], [GoodId]) VALUES (15, 9, N'', N'', N'', N'', 1001)
INSERT INTO [Fuel].[VoucherSetingDetails] ([Id], [VoucherSetingId], [VoucherCeditRefDescription], [VoucherDebitDescription], [VoucherDebitRefDescription], [VoucherCreditDescription], [GoodId]) VALUES (16, 9, N'', N'', N'', N'', 1002)
INSERT INTO [Fuel].[VoucherSetingDetails] ([Id], [VoucherSetingId], [VoucherCeditRefDescription], [VoucherDebitDescription], [VoucherDebitRefDescription], [VoucherCreditDescription], [GoodId]) VALUES (17, 10, N'', N'', N'', N'', 3001)
INSERT INTO [Fuel].[VoucherSetingDetails] ([Id], [VoucherSetingId], [VoucherCeditRefDescription], [VoucherDebitDescription], [VoucherDebitRefDescription], [VoucherCreditDescription], [GoodId]) VALUES (18, 10, N'', N'', N'', N'', 3002)
SET IDENTITY_INSERT [Fuel].[VoucherSetingDetails] OFF


SET IDENTITY_INSERT [Fuel].[AsgnVoucherAconts] ON
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (1, 3, 2, 1)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (2, 3, 5, 2)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (3, 4, 2, 1)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (4, 4, 3, 2)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (5, 5, 1, 1)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (6, 5, 3, 2)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (7, 6, 1, 1)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (8, 6, 5, 2)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (9, 7, 6, 1)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (10, 7, 1, 2)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (11, 8, 7, 1)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (12, 8, 2, 2)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (21, 13, 9, 1)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (22, 13, 7, 2)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (23, 14, 9, 1)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (24, 14, 6, 2)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (25, 15, 10, 1)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (26, 15, 2, 2)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (27, 16, 10, 1)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (28, 16, 1, 2)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (29, 17, 9, 1)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (30, 17, 2, 2)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (31, 18, 9, 1)
INSERT INTO [Fuel].[AsgnVoucherAconts] ([Id], [VoucherSetingDetailId], [AccountId], [Type]) VALUES (32, 18, 1, 2)
SET IDENTITY_INSERT [Fuel].[AsgnVoucherAconts] OFF


SET IDENTITY_INSERT [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ON
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (6, 2, 3, 4)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (7, 1, 4, 1)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (8, 2, 4, 4)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (9, 1, 5, 1)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (10, 2, 5, 4)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (11, 1, 3, 1)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (12, 1, 6, 1)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (13, 2, 6, 4)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (14, 1, 7, 1)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (15, 1, 7, 2)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (16, 1, 7, 3)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (17, 2, 7, 1)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (18, 1, 8, 1)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (19, 1, 8, 2)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (20, 1, 8, 3)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (21, 2, 8, 1)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (38, 1, 13, 4)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (39, 2, 13, 1)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (40, 2, 13, 2)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (41, 2, 13, 3)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (42, 1, 14, 4)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (43, 2, 14, 1)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (44, 2, 14, 2)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (45, 2, 14, 3)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (46, 1, 15, 4)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (47, 2, 15, 1)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (48, 1, 16, 4)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (49, 2, 16, 1)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (50, 1, 17, 4)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (51, 2, 17, 1)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (52, 1, 18, 4)
INSERT INTO [Fuel].[AsgnSegmentTypeVoucherSetingDetail] ([Id], [Type], [VoucherSetingDetailId], [SegmentTypeId]) VALUES (53, 2, 18, 1)
SET IDENTITY_INSERT [Fuel].[AsgnSegmentTypeVoucherSetingDetail] OFF

