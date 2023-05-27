using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mono.Data.Sqlite;

public class Database
{
    private string dbname = "URI=file:GameData.db";
    public static string mapName;
    public const string DIRT_BLOCK = "Dirt block";
    public const string SAND_BLOCK = "Sand block";

    public class Tile
    {
        public string tileName;
        public int posX;
        public int posY;

        public Vector3Int Position()
        {
            return new Vector3Int(posX, posY, 0);
        }
    }

    public Database()
    {
        using (var connection = new SqliteConnection(dbname))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS \"TilePositions\" (" +
                    "\"id\"    INTEGER NOT NULL, " +
                    "\"tile_id\"   INTEGER NOT NULL, " +
                    "\"pos_x\" INTEGER NOT NULL, " +
                    "\"pos_y\" INTEGER NOT NULL, " +
                    "\"map_id\"    INTEGER NOT NULL, " +
                    "FOREIGN KEY(\"tile_id\") REFERENCES \"Tiles\"(\"id\"), " +
                    "FOREIGN KEY(\"map_id\") REFERENCES \"Maps\"(\"id\"), " +
                    "PRIMARY KEY(\"id\" AUTOINCREMENT)); ";

                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS \"Tiles\" (" +
                    "\"id\"    INTEGER NOT NULL, " +
	                "\"name\"  TEXT NOT NULL UNIQUE, " +
	                "\"category_id\"   INTEGER NOT NULL, " +
	                "FOREIGN KEY(\"category_id\") REFERENCES \"Categories\"(\"id\"), " +
	                "PRIMARY KEY(\"id\" AUTOINCREMENT)); ";

                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS \"Maps\" ( " +
                    "\"id\"    INTEGER NOT NULL, " +
	                "\"name\"  TEXT NOT NULL UNIQUE, " +
                    "\"background_id\"  INTEGER NOT NULL, " +
                    "FOREIGN KEY(\"background_id\") REFERENCES \"Backgrounds\"(\"id\"), " +
                    "PRIMARY KEY(\"id\" AUTOINCREMENT)); ";

                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS \"Categories\" ( " +
                    "\"id\"    INTEGER NOT NULL, " +
	                "\"name\"  TEXT NOT NULL UNIQUE, " +
	                "PRIMARY KEY(\"id\" AUTOINCREMENT)); ";

                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS \"Backgrounds\" ( " +
                    "\"id\"    INTEGER NOT NULL, " +
                    "\"name\"  TEXT NOT NULL UNIQUE, " +
                    "PRIMARY KEY(\"id\" AUTOINCREMENT)); ";

                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT OR IGNORE INTO Categories (name) VALUES (\"Block\"); " +
                    "INSERT OR IGNORE INTO Categories (name) VALUES (\"Decoration\"); " +
                    "INSERT OR IGNORE INTO Backgrounds (name) VALUES (\"Day\"); " +
                    "INSERT OR IGNORE INTO Backgrounds (name) VALUES (\"Night\"); " +
                    "INSERT OR IGNORE INTO Tiles(name, category_id) VALUES(\"Dirt block\", 1); " +
                    "INSERT OR IGNORE INTO Tiles(name, category_id) VALUES(\"Sand block\", 1); ";

                command.ExecuteNonQuery();
            } 

            connection.Close();
        }
    }
    
    public List<string> GetMapNames()
    {
        List<string> MapNames = new List<string>();
        //make list of names from Maps table
        using (var connection = new SqliteConnection(dbname))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT name FROM Maps; ";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader["name"].ToString();
                        MapNames.Add(name);

                        Debug.Log("Name: " + reader["name"]);
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
        return MapNames;
    }

    public List<Tile> LoadTiles(string mapName)
    {
        List<Tile> TilePositions = new List<Tile>();
        //show tiles and its positions where name is map_id
        using (var connection = new SqliteConnection(dbname))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT pos_x, pos_y, Tiles.name FROM TilePositions " +
                    "LEFT JOIN Maps on Maps.id = TilePositions.map_id " +
                    "LEFT JOIN Tiles on Tiles.id = TilePositions.tile_id " +
                    "WHERE Maps.name = $name; ";
                command.Parameters.AddWithValue("$name", mapName);

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tile tile = new Tile();

                        tile.posX = reader.GetInt32(0);
                        tile.posY = reader.GetInt32(1);
                        tile.tileName = reader.GetString(2);

                        TilePositions.Add(tile);
                    }
                    reader.Close();
                }
            }

            connection.Close();
        }
        return TilePositions;
    }

    public string LoadBackground(string mapName)
    {
        string backgroundName;
        using (var connection = new SqliteConnection(dbname))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Backgrounds.name FROM Backgrounds " +
                    "LEFT JOIN Maps ON Backgrounds.id = background_id " +
                    "WHERE Maps.name = $name; ";
                command.Parameters.AddWithValue("$name", mapName);

                backgroundName = Convert.ToString(command.ExecuteScalar());
            }

            connection.Close();
        }
        return backgroundName;
    }

    public void SaveMap(string mapName, List<Tile> tiles, string backgroundName)
    {
        //insert tiles positions in TilesPositions table after inserting map name
        using (var connection = new SqliteConnection(dbname))
        {
            connection.Open();

            int map_id;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id FROM Maps WHERE name = $name;";
                command.Parameters.AddWithValue("$name", mapName);

                var result = command.ExecuteScalar();

                if (result != null)
                {
                    map_id = Convert.ToInt32(result);
                    using (var command1 = connection.CreateCommand())
                    {
                        command1.CommandText = "UPDATE Maps SET background_id = $background_id WHERE id = $map_id;";
                        command1.Parameters.AddWithValue("$map_id", map_id);
                        command1.Parameters.AddWithValue("$background_id", GetBackgroundId(backgroundName));

                        command1.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (var command1 = connection.CreateCommand())
                    {
                        command1.CommandText = "INSERT INTO Maps (name, background_id) VALUES ($name, $background_id) RETURNING id;";
                        command1.Parameters.AddWithValue("$name", mapName);
                        command1.Parameters.AddWithValue("$background_id", GetBackgroundId(backgroundName));

                        map_id = Convert.ToInt32(command1.ExecuteScalar());
                    }
                }
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM TilePositions WHERE map_id = $map_id";
                command.Parameters.AddWithValue("$map_id", map_id);

                var deleted = command.ExecuteNonQuery();
                Debug.Log("Deleted: " + deleted);
            }

            foreach (Tile tile in tiles)
            {
                using (var command = connection.CreateCommand())
                {
                    switch (tile.tileName)
                    {
                        case DIRT_BLOCK:
                            command.Parameters.AddWithValue("$tile_id", 1);
                            break;
                        case SAND_BLOCK:
                            command.Parameters.AddWithValue("$tile_id", 2);
                            break;
                        default:
                            break;
                    }
                    command.CommandText = "INSERT INTO TilePositions (tile_id, pos_x, pos_y, map_id) " +
                                "VALUES ($tile_id, $posX, $posY, $map_id);";

                    command.Parameters.AddWithValue("$posX", tile.posX);
                    command.Parameters.AddWithValue("$posY", tile.posY);
                    command.Parameters.AddWithValue("$map_id", map_id);

                    command.ExecuteNonQuery();
                } 
            }

            connection.Close();
        }
    }

    private int GetBackgroundId(string backgroundName)
    {
        int result;
        using (var connection = new SqliteConnection(dbname))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id FROM Backgrounds WHERE name = $name;";
                command.Parameters.AddWithValue("$name", backgroundName);

                result = Convert.ToInt32(command.ExecuteScalar());
            }

            connection.Close();
        }
        return result;
    }

}
