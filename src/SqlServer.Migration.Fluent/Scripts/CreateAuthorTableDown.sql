ALTER TABLE Article add Authors nvarchar(max);

select ArticleId as Id ,STRING_AGG(Author.Name, ', ') as Authors
  into #Updated
  from Author
  GROUP by ArticleId;

Update Article
    Set Authors = #Updated.Authors
from Article 
join #Updated on article.id = #Updated.Id;

DROP TABLE Author;