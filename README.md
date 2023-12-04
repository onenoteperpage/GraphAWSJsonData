## Reading CPU Data

CPU data is stored as a 0.1% value, rather than a 1.0% value in the SqliteDB. To counter this, when the data  being converted from the SqliteDB to JSON occurs, it takes this into account and multiplies the value by 10x.

Originally, the data when dumped would be:
```json
{
    "SDataTable": "CPU",
    "SDataPoints": [
      {
        "Timestamp": "2023-11-13T16:37:37",
        "Value": 1.7815580368041992
      },
```

Becomes corrected during the SQLiteDB data export to be:

```json
{
    "SDataTable": "CPU",
    "SDataPoints": [
      {
        "Timestamp": "2023-11-13T16:37:37",
        "Value": 17.815580368041992
      },
```

## Reading x_Drive Data

The same rule for `CPU` data also applies to `x_Drive` data. This is caused by the way the `WinBoxStatsChecker` reads the data from the WinAPI, and I have no intention of fixing it in the source metrics for the forseeable future.