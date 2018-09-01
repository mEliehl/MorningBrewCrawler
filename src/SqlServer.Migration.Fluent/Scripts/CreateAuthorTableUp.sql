Create Table Author(
    ArticleId uniqueidentifier,
    Name NVARCHAR(max),
    FOREIGN KEY (ArticleId) REFERENCES Article(Id) ON DELETE CASCADE
);

CREATE TABLE #CommaDelimiter
(
	[Id] [uniqueidentifier] NOT NULL,	
	[Authors] [nvarchar](max) NOT NULL
)

Insert Into #CommaDelimiter
select Id ,REPLACE(value,' ','') as Authors
from Article
CROSS APPLY string_split(Authors,',')

insert into Author
select Id as ArticleId, REPLACE(value,' ','') as Name
from #CommaDelimiter
CROSS APPLY string_split(Authors,'&');

ALTER TABLE Article DROP COLUMN Authors;