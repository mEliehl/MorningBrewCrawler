use MorningBrew;

create table Article(
    Id uniqueidentifier primary key,

    Date DATETIME,
    Link VARCHAR(max),
    Title VARCHAR(max),
    Authors VARCHAR(max)
)