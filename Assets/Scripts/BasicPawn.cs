using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPawn : PawnScript
{
    public override List<Step> PossibleSteps() {
        
        var moves = new List<Step>();
        int posX = 0, posY = 0;

        if(IsWhite) {
             // diaglonal left + 1
            posX = this.coordinate.x - 1;
            posY = this.coordinate.y - 1;
            base.Step(posX, posY, ref moves);
            if(moves.Count > 0) {
                if(moves[moves.Count - 1].impact) {
                    // diaglonal left + 2
                    posX = this.coordinate.x - 2;
                    posY = this.coordinate.y - 2;
                    base.Step(posX, posY, ref moves);
                }
            }

            // diaglonal right + 1
            posX = this.coordinate.x + 1;
            posY = this.coordinate.y - 1;
            base.Step(posX, posY, ref moves);
 
            if(moves.Count > 0) {
                if(moves[moves.Count - 1].impact) {
                    // diaglonal right + 2
                    posX = this.coordinate.x + 2;
                    posY = this.coordinate.y - 2;
                    base.Step(posX, posY, ref moves);
                }
            }
        } 
        else {
            // diaglonal left + 1
            posX = this.coordinate.x - 1;
            posY = this.coordinate.y +1;
            base.Step(posX, posY, ref moves);

            if(moves.Count > 0) {
                if(moves[moves.Count - 1].impact) {
                    // diaglonal left + 2
                    posX = this.coordinate.x - 2;
                    posY = this.coordinate.y + 2;
                    base.Step(posX, posY, ref moves);
                }
            }

            // diaglonal right + 1
            posX = this.coordinate.x + 1;
            posY = this.coordinate.y +1;
            base.Step(posX, posY, ref moves);
           
            if(moves.Count > 0) {
                if(moves[moves.Count - 1].impact) { 
                    // diaglonal right + 2
                    posX = this.coordinate.x + 2;
                    posY = this.coordinate.y + 2;
                    base.Step(posX, posY, ref moves);
                }
            }
        }
        
        return moves;
    }
}
