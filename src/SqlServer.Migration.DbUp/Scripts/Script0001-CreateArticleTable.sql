use MorningBrew;

create table Article(
    Id uniqueidentifier primary key,

    Date DATETIME,
    Link NVARCHAR(max),
    Title NVARCHAR(max),
    Authors NVARCHAR(max)
)