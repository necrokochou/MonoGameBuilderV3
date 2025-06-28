using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace Core;


public class Builder {
    // FIELDS
    private Stage stage;


    // CONSTRUCTORS
    internal Builder(Stage stage) {
        this.stage = stage;
    }


    // PROPERTIES
    // --- INSTANCE PROPERTIES ---
    private Background Background {
        set => stage.Background = value;
    }
    private Tile[,] Tiles {
        get => stage.Tiles;
    }
    public Character Character {
        get => stage.Character;
        internal set => stage.Character = value;
    }
    private int Rows {
        get => stage.Rows;
    }
    private int Columns {
        get => stage.Columns;
    }


    // METHODS
    // --- INSTANCE METHODS ---
    public void SetBackground(Background background) {
        Background = background;
    }

    public void SetCharacter(Character character, int row, int column) {
        ValidatePosition(row, column);
        
        character.Stage = stage;
        character.SetLocation(row, column);
        
        Character = character;
    }
    
    public void AddTile(Tile tile, int row, int column) {
        ValidatePosition(row, column);
        
        Tile newTile = Tile.Copy(tile);
        newTile.SetLocation(row, column);
        
        Tiles[row,column] = newTile;
    }

    public void AddTiles(Tile tile, int startRow, int endRow, int startColumn, int endColumn) {
        for (int row = startRow; row <= endRow; row++) {
            for (int column = startColumn; column <= endColumn; column++) {
                ValidatePosition(row, column);
                
                AddTile(tile, row, column);
            }
        }
    }
    
    public void AddFloor(Tile tile, int row, int column, int width, int height = 1) {
        if (height - 1 > width / 2)
            throw new ArgumentException("Height must be less than width");
        
        AddTiles(tile, row, row + height - 1, column, column + width - 1);
    }

    public void AddWall(Tile tile, int row, int column, int height, int width = 1) {
        if (width - 1 > height / 2)
            throw new ArgumentException("Width must be less than height");
        
        AddTiles(tile, row - height + 1, row + 1, column, column + width);
    }

    
    // --- EXCEPTION METHODS ---
    private void ValidatePosition(int row, int column) {
        if (row < 0 || row >= Rows)
            throw new ArgumentException($"Row must be between 0 and {Rows - 1}: ({row},{column})");
        
        if (column < 0 || column >= Columns)
            throw new ArgumentException($"Column must be between 0 and {Columns - 1}: ({row},{column})");
        
        if (Tiles[row,column] != null)
            throw new ArgumentException($"Tile position already occupied: ({row},{column})");
    }
}
