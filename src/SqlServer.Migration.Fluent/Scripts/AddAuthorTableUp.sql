CREATE TABLE #CommaDelimiter
(
	[Id] [uniqueidentifier] NOT NULL,	
	[Authors] [nvarchar](max) NOT NULL
 
)

Insert Into #CommaDelimiter
select Id ,TRIM(value) as Authors
from Article
CROSS APPLY string_split(Authors,',')

insert into Author
select Id as ArticleId, TRIM(value) as Name
from #CommaDelimiter
CROSS APPLY string_split(Authors,'&');