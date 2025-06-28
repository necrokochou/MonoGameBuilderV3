using System;
using Microsoft.Xna.Framework;


namespace Core;


public class Animation {
    // FIELDS
    private Character character;
    private int counter;
    private int frames;
    private int interval;
    private int currentFrame;
    private int columnPos;
    private int rowPos;

    private Point startFrame;
    private Point endFrame;


    // CONSTRUCTORS
    internal Animation(Character character, int interval, Point startFrame, Point endFrame) {
        this.character = character;
        this.interval = interval;
        this.startFrame = startFrame;
        this.endFrame = endFrame;
        frames = endFrame.Y - startFrame.Y + 1;
    }


    // PROPERTIES
    private int SourceWidth {
        get => character.Source.Width;
    }
    private int SourceHeight {
        get => character.Source.Height;
    }


    // METHODS
    // --- MAIN METHODS ---
    internal void Update() {
        counter++;
        if (counter > interval) {
            counter = 0;
            NextFrame();
        }
    }
    
    // --- STATIC METHODS ---
    public static Animation Create(Character character, int interval, Point startFrame, Point endFrame) {
        return new(character, interval, startFrame, endFrame);
    }
    
    // --- INSTANCE METHODS ---
    private void NextFrame() {
        currentFrame++;
        columnPos++;

        if (columnPos > endFrame.Y) {
            columnPos = 0;
            rowPos++;
        }
        
        if (rowPos > endFrame.X) Reset();
        
        Console.WriteLine("Frame: " + currentFrame + " | Column: " + columnPos + " | Row: " + rowPos + " |");
    }
    
    private void Reset() {
        currentFrame = 0;
        rowPos = startFrame.X;
        columnPos = startFrame.Y;
        
    }

    public Rectangle GetFrame() {
        return new Rectangle(
            columnPos * SourceWidth,
            rowPos * SourceHeight,
            SourceWidth,
            SourceHeight
        );
    }
}
