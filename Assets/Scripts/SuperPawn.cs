using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPawn : PawnScript
{
        public override List<Step> PossibleSteps() {
        
        var moves = new List<Step>();
        int posX = 0, posY = 0;


            // diagonale right
            posX = this.coordinate.x;
            posY = this.coordinate.y;
            while( posX <= 7 && posY >= 0 ) {
                ++posX;
                --posY;
                base.Step(posX, posY, ref moves);
                if(moves.Count > 0) {
                    if(moves[moves.Count - 1].impact) {
                        base.Step(posX + 1, posY - 1, ref moves);
                        break;
                    }
                }
            }

            // diagonale left
            posX = this.coordinate.x;
            posY = this.coordinate.y;
            while( posX >= 1 && posY >= 1 ) {
                --posX;
                --posY;
                base.Step(posX, posY, ref moves);
                if(moves.Count > 0) {
                    if(moves[moves.Count - 1].impact) {
                        base.Step(posX - 1, posY - 1, ref moves);
                        break;
                    }
                }
            }

            // diagonale right
            posX = this.coordinate.x;
            posY = this.coordinate.y;
            while( posX <= 7 && posY <= 7 ) {
                ++posX;
                ++posY;
                base.Step(posX, posY, ref moves);
                if(moves.Count > 0) {
                    if(moves[moves.Count - 1].impact) {
                        base.Step(posX + 1, posY + 1, ref moves);
                        break;
                    }
                }
            }

            // diagonale left
            posX = this.coordinate.x;
            posY = this.coordinate.y;
            while( posX >= 1 && posY <= 8 ) {
                --posX;
                ++posY;
                base.Step(posX, posY, ref moves);
                if(moves.Count > 0) {
                    if(moves[moves.Count - 1].impact) {
                        base.Step(posX - 1, posY + 1, ref moves);
                        break;
                    }
                }
            }
        

        return moves;
    }
}
