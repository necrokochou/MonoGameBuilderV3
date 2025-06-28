using System;
using Microsoft.Xna.Framework;


namespace Core;


public class Animation {
    // FIELDS
    private Character character;
    private int counter;
    private int interval;
    private int columnPos;
    private int rowPos;

    private Point startFrame;
    private Point endFrame;


    // CONSTRUCTORS
    internal Animation(Character character, Point startFrame, Point endFrame, int interval) {
        this.character = character;
        this.startFrame = startFrame;
        this.endFrame = endFrame;
        this.interval = interval;
    }


    // PROPERTIES
    private int SourceWidth {
        get => character.Source.Width;
    }
    private int SourceHeight {
        get => character.Source.Height;
    }
    private int Rows {
        get => character.Rows;
    }
    private int Columns {
        get => character.Columns;
    }


    // METHODS
    // --- MAIN METHODS ---
    internal void Update() {
        counter++;
        if (counter >= interval) {
            counter = 0;
            NextFrame();
        }
    }
    
    // --- STATIC METHODS ---
    public static Animation Create(Character character, Point startFrame, Point endFrame, int interval) {
        return new(character, startFrame, endFrame, interval);
    }
    
    // --- INSTANCE METHODS ---
    private void NextFrame() {
        columnPos++;

        if (columnPos >= Columns) {
            columnPos = 0;
            rowPos++;
        }
        
        if (rowPos > endFrame.X || (rowPos == endFrame.X && columnPos > endFrame.Y)) Reset();
        
    }
    
    private void Reset() {
        rowPos = startFrame.X;
        columnPos = startFrame.Y;
        
    }

    internal Rectangle GetFrame() {
        return new(
            columnPos * SourceWidth,
            rowPos * SourceHeight,
            SourceWidth,
            SourceHeight
        );
    }
}
