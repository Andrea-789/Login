
/****** Object:  Table [dbo].[Users]    Script Date: 28/05/2022 10:04:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[Id] [nvarchar](50) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Surname] [varchar](100) NOT NULL,
	[Address] [varchar](200) NOT NULL,
	[ZIP] [varchar](5) NOT NULL,
	[City] [varchar](100) NOT NULL,
	[State] [varchar](2) NOT NULL,
	[Telephone] [varchar](30) NOT NULL,
	[CompanyName] [varchar](100) NOT NULL,
	[STN] [varchar](11) NOT NULL,
	[SSN] [varchar](16) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[Enabled] [tinyint] NOT NULL,
	[Date] [date] NOT NULL,
	[Role] [varchar](10) NOT NULL,
 CONSTRAINT [PK_Users_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Id]  DEFAULT ('') FOR [Id]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Noae]  DEFAULT ('') FOR [Name]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Surname]  DEFAULT ('') FOR [Surname]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Address]  DEFAULT ('') FOR [Address]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_ZIP]  DEFAULT ('') FOR [ZIP]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_City]  DEFAULT ('') FOR [City]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_State]  DEFAULT ('') FOR [State]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Telephone]  DEFAULT ('') FOR [Telephone]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_CompanyName]  DEFAULT ('') FOR [CompanyName]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_STN]  DEFAULT ('') FOR [STN]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_SSN]  DEFAULT ('') FOR [SSN]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Email]  DEFAULT ('') FOR [Email]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Password]  DEFAULT ('') FOR [Password]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Enabled]  DEFAULT ((0)) FOR [Enabled]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Date]  DEFAULT ('') FOR [Date]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Role]  DEFAULT ('') FOR [Role]
GO


