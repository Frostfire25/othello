using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Othello
{

    /*
     * 
     * Othello
     * Alex Elguezabal
     * 1/13/2019
     * 
     */

    public partial class Othello : Form
    {
        //Boolean for if a game is active
        private bool playing;
        //True if it is blacks turn, false if not
        private bool black_turn;

        //Amount of pieces that black or white has left
        private int black_pieces_left;
        private int white_pieces_left;

        //2D array for the 8x8 board.
        private int[,] board;

        private int spaces_set;

        private bool passed;


        /*
         * 
         * Empty spaces on the board are set to -1
         * Black spaces are set to 0
         * White spaces are set to 1
         * 
         */


        public Othello()
        {
            InitializeComponent();
            newGame();
            scoreBoxBlack.Image = Properties.Resources.black;
            scoreBoxWhite.Image = Properties.Resources.white;
        }

        private void newGame()
        {
            if (playing)
            {
                clearBoard();
            }

            passed = true;
            this.playing = true;
            this.black_pieces_left = 30;
            this.white_pieces_left = 30;

            //Made the board extra big to prevent out of bounds errors
            this.board = new int[8, 8];

            //Sets each space to an empty space.
            for (int i = 0; i < 8; i++)
            {
                for (int q = 0; q < 8; q++)
                {
                    this.board[i, q] = -1;
                }
            }

            //Sets the four spaces in the middle
            setSpace(3, 3, 0, box28);
            setSpace(3, 4, 1, box29);
            setSpace(4, 3, 0, box36);
            setSpace(4, 4, 1, box37);

            //Sets the starting score
            scoreBlack.Text = "" + 2;
            scoreWhite.Text = "" + 2;

            //Sets the game to blacks turn
            this.black_turn = true;
            //Sets the winnerLabel to hidden
            winnerLabel.Visible = false;
        }


        /*
         * SetSpace Method
         * 
         * @title: SetSpace
         * @description: Used to set the spaces on the board when clicked on by the user.
         * @param: row: row of the space
         * @param: colum: column of the space
         * @param: color of the space to change the color.
         * 
         */

        private bool setSpace(int row, int colum, int color, PictureBox pictureBox)
        {
            passed = true;
            if(spaces_set > 3)
            {
                int opposite = 1;
                if (color == 1)
                    opposite = 0;
                if(!isOppositeNextTo(row,colum,opposite))
                {
                    passed = false;
                    return false;
                }
            }       
            pictureBox = getBoxFromRowAndColum(row, colum);
            if (color == 0) //Black
            {
                this.board[row, colum] = 0;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Image = Properties.Resources.black;

            }
            else //White 
            {
                this.board[row, colum] = 1;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Image = Properties.Resources.white;

            }
            spaces_set++;
            return true;
        }

        private bool isOppositeNextTo(int row, int colum, int opposite)
        {
            try
            {
                if (this.board[row - 1, colum - 1] == opposite)
                {
                    return true;
                }
            }
            catch (IndexOutOfRangeException ee)
            {

            }

            try
            {
                if (this.board[row - 1, colum + 1] == opposite)
                {
                    return true;
                }
            }
            catch (IndexOutOfRangeException ee)
            {

            }

            try
            {
                if (this.board[row - 1, colum] == opposite)
                {
                    return true;
                }
            }
            catch (IndexOutOfRangeException ee)
            {

            }

            try
            {
                if (this.board[row + 1, colum - 1] == opposite)
                {
                    return true;
                }
            }
            catch (IndexOutOfRangeException ee)
            {

            }

            try
            {
                if (this.board[row, colum - 1] == opposite)
                {
                    return true;
                }
            }
            catch (IndexOutOfRangeException ee)
            {

            }

            try
            {
                if (this.board[row + 1, colum - 1] == opposite)
                {
                    return true;
                }
            }
            catch (IndexOutOfRangeException ee)
            {

            }

            try
            {
                if (this.board[row + 1, colum + 1] == opposite)
                {
                    return true;
                }
            }
            catch (IndexOutOfRangeException ee)
            {

            }

            try
            {
                if (this.board[row + 1, colum] == opposite)
                {
                    return true;
                }
            }
            catch (IndexOutOfRangeException ee)
            {

            }




            return false;
        }

        //Gets the amount of Black controlled spaces on the board.
        private int getBlacks()
        {
            int count = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int q = 0; q < 8; q++)
                {
                    if (this.board[i, q] == 0)
                        count++;
                }
            }

            return count;
        }

        //Gets the amount of White controlled spaces on the board.
        private int getWhites()
        {
            int count = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int q = 0; q < 8; q++)
                {
                    if (this.board[i, q] == 1)
                        count++;
                }
            }

            return count;
        }

        /*
         * CheckForOverTurns Method
         * 
         * @title: CheckForOverTurns
         * @description: On each move the method is called and it checks to see if any spaces can be overturned.
         * @param: row: row of the space.
         * @param: colum: column of the space.
         * 
         */

        private void checkForOverTurns(int row, int colum)
        {
            if (!passed)
                return;

            //Color of the space
            int color = this.board[row, colum];
            //If the space is empty it returns
            if (color == -1)
                return;

            //Goes from the space, left and checks the rows, if one has the space at the end it flips.
            int s1 = row - 1;
            int ss1 = -1;
            for (int i = s1; i > -1; i--)
            {
                if (this.board[i, colum] == -1)
                {
                    break;
                }
                else if (this.board[i, colum] == color)
                {
                    ss1 = i;
                    for (int q = s1; q > ss1; q--)
                    {
                        setSpace(q, colum, color, null);
                    }
                }
            }

            //Goes from the space, right and checks the rows, if one has the space at the end it flips.
            int s2 = row + 1;
            int ss2 = -1;
            for (int i = s2; i < 8; i++)
            {
                if (this.board[i, colum] == -1)
                {
                    break;
                }
                else if (this.board[i, colum] == color)
                {
                    ss2 = i;
                    for (int q = s2; q < ss2; q++)
                    {
                        setSpace(q, colum, color, null);
                    }
                }
            }

            //Goes from the space, down and checks the columns, if one has the space at the end it flips.
            int s3 = colum - 1;
            int ss3 = -1;
            for (int i = s3; i > -1; i--)
            {
                if (this.board[row, i] == -1)
                {
                    break;
                }
                else if (this.board[row, i] == color)
                {
                    ss3 = i;
                    for (int q = s3; q > ss3; q--)
                    {
                        setSpace(row, q, color, null);
                    }
                }
            }

            //Goes from the space, up and checks the columns, if one has the space at the end it flips.
            int s4 = colum + 1;
            int ss4 = -1;
            for (int i = s4; i < 8; i++)
            {
                if (this.board[row, i] == -1)
                {
                    break;
                }
                else if (this.board[row, i] == color)
                {
                    ss4 = i;
                    for (int q = s4; q < ss4; q++)
                    {
                        setSpace(row, q, color, null);
                    }
                }
            }

            //
            /*
            int s5 = row + 1;
            int ss5row = 0;
            int ss5colum = 0;
            int ss5 = colum + 1;
            int ii = colum + 1;
            for (int i = s5; i < 8; i++)
            {
                try
                {
                    if (this.board[i, ii++] == -1)
                    {
                        return;
                    }
                    else if (this.board[i, ii] == color)
                    {
                        ss5row = i;
                        ss5colum = ii;
                        for (int q = s5; q < ss5row; q++)
                        {
                            setSpace(q, ss5++, color, null);
                        }
                    }
                }
                catch (IndexOutOfRangeException ee)
                {
                    break;
                }


            }*/

            MessageBox.Show(row + " " + colum);

            int start_row = row + 1;
            int start_colum = colum + 1;
            int q1 = start_colum;
            for(int i = start_row; i < 8;)
            {
                try
                {
                    if (this.board[i, q1] == -1)
                    {
                        break;
                    }
                    else if (this.board[i, q1] == color)
                    {
                        int qq = colum;
                        for (int ii = row; ii < i;)
                        {
                            MessageBox.Show("ran");
                            setSpace(ii, qq, color, null);
                            qq++;
                            ii++;
                        }
                    }
                    MessageBox.Show(""+i+" "+q1);
                } catch(Exception ee)
                {
                    break;
                }
                q1++;
                i++;
            }

            //works top down left to right

        }


        //Switches the turn
        private void switchTurn()
        {
            if (!passed)
                return;
            //Changes the turn and sets the pieces to -1;
            if (black_turn)
            {
                white_pieces_left--;
                black_turn = false;
            }
            else
            {
                black_pieces_left--;
                black_turn = true;
            }

            //Updates the score card
            scoreBlack.Text = "" + getBlacks();
            scoreWhite.Text = "" + getWhites();

            //If the board is filled it runs the endGame() method
            if (white_pieces_left == 0 && black_pieces_left == 0 || boardFilled())
            {
                int winner = -2;
                if (getBlacks() < getWhites())
                {
                    winner = 1;
                }
                else if (getBlacks() > getWhites())
                {
                    winner = 0;
                }
                else
                {
                    winner = -1;
                }
                endGame(winner);
            }
        }

        //Returns true if the board is filled.
        private bool boardFilled()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int q = 0; q < 8; q++)
                {
                    if (this.board[i, q] == -1)
                        return false;
                }
            }
            return true;
        }

        //Clears the board
        private void clearBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int q = 0; q < 8; q++)
                {
                    this.board[i, q] = -1;
                    PictureBox box = getBoxFromRowAndColum(i, q);
                    box.Image = null;
                }
            }
        }

        //Needs to be finished
        private void endGame(int winner /* 0 == black 1 == white -1 == tie */)
        {
            switch (winner)
            {
                case 1:
                    {
                        winnerLabel.BackColor = Color.Black;
                        winnerLabel.ForeColor = Color.White;
                        winnerLabel.Text = "Winner: White!";
                    }
                    break;
                case 0:
                    {
                        winnerLabel.BackColor = Color.White;
                        winnerLabel.ForeColor = Color.Black;
                        winnerLabel.Text = "Winner: Black!";
                    }
                    break;
                case -1:
                    {
                        winnerLabel.BackColor = Color.Yellow;
                        winnerLabel.ForeColor = Color.Blue;
                        winnerLabel.Text = "Winner: Tie!";
                    }
                    break;
            }
            winnerLabel.Visible = true;
        }




        /*
         * Listeners
         */

        private void GetPiecesLeft_Click(object sender, EventArgs e)
        {
            MessageBox.Show("" + black_pieces_left + "  " + white_pieces_left);
        }

        private void Othello_FormClosed(object sender, FormClosedEventArgs e)
        {
            //exit application when form is closed
            Application.Exit();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            clearBoard();
            newGame();
        }

        /*
         * 1st Row Listeners
         */

        private void Box1_Click(object sender, EventArgs e)
        {
            //if() //Check to see if space is == -1, if is continue

            if (this.board[0, 0] != -1)
                return;


            if (black_turn)
            {
               setSpace(0, 0, 0, box1);
            }
            else
            {
                setSpace(0, 0, 1, box1);
            }


            checkForOverTurns(0, 0);
            switchTurn();
        }

        private void Box2_Click(object sender, EventArgs e)
        {
            if (this.board[1, 0] != -1)
                return;

            if (black_turn)
            {
                setSpace(1, 0, 0, box1);
            }
            else
            {
               setSpace(1, 0, 1, box1);
            }


            checkForOverTurns(1, 0);
            switchTurn();
        }

        private void Box3_Click(object sender, EventArgs e)
        {
            if (this.board[2, 0] != -1)
                return;

            if (black_turn)
            {
                setSpace(2, 0, 0, box1);
            }
            else
            {
                setSpace(2, 0, 1, box1);
            }


            checkForOverTurns(2, 0);
            switchTurn();
        }

        private void Box4_Click(object sender, EventArgs e)
        {
            if (this.board[3, 0] != -1)
                return;

            if (black_turn)
            {
                setSpace(3, 0, 0, box1);
            }
            else
            {
                setSpace(3, 0, 1, box1);
            }

            checkForOverTurns(3, 0);
            switchTurn();
        }

        private void Box5_Click(object sender, EventArgs e)
        {
            if (this.board[4, 0] != -1)
                return;

            if (black_turn)
            {
                setSpace(4, 0, 0, box1);
            }
            else
            {
                setSpace(4, 0, 1, box1);
            }

            checkForOverTurns(4, 0);
            switchTurn();
        }

        private void Box6_Click(object sender, EventArgs e)
        {
            if (this.board[5, 0] != -1)
                return;

            if (black_turn)
            {
                setSpace(5, 0, 0, box1);
            }
            else
            {
                setSpace(5, 0, 1, box1);
            }

            checkForOverTurns(5, 0);
            switchTurn();
        }

        private void Box7_Click(object sender, EventArgs e)
        {
            if (this.board[6, 0] != -1)
                return;

            if (black_turn)
            {
                setSpace(6, 0, 0, box1);
            }
            else
            {
                setSpace(6, 0, 1, box1);
            }

            checkForOverTurns(6, 0);
            switchTurn();
        }

        private void Box8_Click(object sender, EventArgs e)
        {
            if (this.board[7, 0] != -1)
                return;

            if (black_turn)
            {
                setSpace(7, 0, 0, box1);
            }
            else
            {
                setSpace(7, 0, 1, box1);
            }

            checkForOverTurns(7, 0);
            switchTurn();
        }

        /*
         * 2nd Row Listeners
         */

        private void Box9_Click(object sender, EventArgs e)
        {
            if (this.board[0, 1] != -1)
                return;

            if (black_turn)
            {
                setSpace(0, 1, 0, box1);
            }
            else
            {
                setSpace(0, 1, 1, box1);
            }

            checkForOverTurns(0, 1);
            switchTurn();
        }

        private void Box10_Click(object sender, EventArgs e)
        {
            if (this.board[1, 1] != -1)
                return;

            if (black_turn)
            {
                setSpace(1, 1, 0, box1);
            }
            else
            {
                setSpace(1, 1, 1, box1);
            }

            checkForOverTurns(1, 1);
            switchTurn();
        }

        private void Box11_Click(object sender, EventArgs e)
        {
            if (this.board[2, 1] != -1)
                return;

            if (black_turn)
            {
                setSpace(2, 1, 0, box1);
            }
            else
            {
                setSpace(2, 1, 1, box1);
            }

            checkForOverTurns(2, 1);
            switchTurn();
        }

        private void Box12_Click(object sender, EventArgs e)
        {
            if (this.board[3, 1] != -1)
                return;

            if (black_turn)
            {
                setSpace(3, 1, 0, box1);
            }
            else
            {
                setSpace(3, 1, 1, box1);
            }

            checkForOverTurns(3, 1);
            switchTurn();
        }

        private void Box13_Click(object sender, EventArgs e)
        {
            if (this.board[4, 1] != -1)
                return;

            if (black_turn)
            {
                setSpace(4, 1, 0, box1);
            }
            else
            {
                setSpace(4, 1, 1, box1);
            }

            checkForOverTurns(4, 1);
            switchTurn();
        }

        private void Box14_Click(object sender, EventArgs e)
        {
            if (this.board[5, 1] != -1)
                return;

            if (black_turn)
            {
                setSpace(5, 1, 0, box1);
            }
            else
            {
                setSpace(5, 1, 1, box1);
            }

            checkForOverTurns(5, 1);
            switchTurn();
        }

        private void Box15_Click(object sender, EventArgs e)
        {
            if (this.board[6, 1] != -1)
                return;

            if (black_turn)
            {
                setSpace(6, 1, 0, box1);
            }
            else
            {
                setSpace(6, 1, 1, box1);
            }

            checkForOverTurns(6, 1);
            switchTurn();
        }

        private void Box16_Click(object sender, EventArgs e)
        {
            if (this.board[7, 1] != -1)
                return;

            if (black_turn)
            {
                setSpace(7, 1, 0, box1);
            }
            else
            {
                setSpace(7, 1, 1, box1);
            }

            checkForOverTurns(7, 1);
            switchTurn();
        }

        /*
         * 3rd Row Listseners 
         */

        private void Box17_Click(object sender, EventArgs e)
        {
            if (this.board[0, 2] != -1)
                return;

            if (black_turn)
            {
                setSpace(0, 2, 0, box1);
            }
            else
            {
                setSpace(0, 2, 1, box1);
            }

            checkForOverTurns(0, 2);
            switchTurn();
        }

        private void Box18_Click(object sender, EventArgs e)
        {
            if (this.board[1, 2] != -1)
                return;

            if (black_turn)
            {
                setSpace(1, 2, 0, box1);
            }
            else
            {
                setSpace(1, 2, 1, box1);
            }

            checkForOverTurns(1, 2);
            switchTurn();
        }

        private void Box19_Click(object sender, EventArgs e)
        {
            if (this.board[2, 2] != -1)
                return;

            if (black_turn)
            {
                setSpace(2, 2, 0, box1);
            }
            else
            {
                setSpace(2, 2, 1, box1);
            }

            checkForOverTurns(2, 2);
            switchTurn();
        }

        private void Box20_Click(object sender, EventArgs e)
        {
            if (this.board[3, 2] != -1)
                return;

            if (black_turn)
            {
                setSpace(3, 2, 0, box1);
            }
            else
            {
                setSpace(3, 2, 1, box1);
            }

            checkForOverTurns(3, 2);
            switchTurn();
        }

        private void Box21_Click(object sender, EventArgs e)
        {
            if (this.board[4, 2] != -1)
                return;

            if (black_turn)
            {
                setSpace(4, 2, 0, box1);
            }
            else
            {
                setSpace(4, 2, 1, box1);
            }

            checkForOverTurns(4, 2);
            switchTurn();
        }

        private void Box22_Click(object sender, EventArgs e)
        {
            if (this.board[5, 2] != -1)
                return;

            if (black_turn)
            {
                setSpace(5, 2, 0, box1);
            }
            else
            {
                setSpace(5, 2, 1, box1);
            }

            checkForOverTurns(5, 2);
            switchTurn();
        }

        private void Box23_Click(object sender, EventArgs e)
        {
            if (this.board[6, 2] != -1)
                return;

            if (black_turn)
            {
                setSpace(6, 2, 0, box1);
            }
            else
            {
                setSpace(6, 2, 1, box1);
            }

            checkForOverTurns(6, 2);
            switchTurn();
        }

        private void Box24_Click(object sender, EventArgs e)
        {
            if (this.board[7, 2] != -1)
                return;

            if (black_turn)
            {
                setSpace(7, 2, 0, box1);
            }
            else
            {
                setSpace(7, 2, 1, box1);
            }

            checkForOverTurns(7, 2);
            switchTurn();
        }

        /*
         * 4th Row
         */

        private void Box25_Click(object sender, EventArgs e)
        {
            if (this.board[0, 3] != -1)
                return;

            if (black_turn)
            {
                setSpace(0, 3, 0, box1);
            }
            else
            {
                setSpace(0, 3, 1, box1);
            }

            checkForOverTurns(0, 3);
            switchTurn();
        }

        private void Box26_Click(object sender, EventArgs e)
        {
            if (this.board[1, 3] != -1)
                return;

            if (black_turn)
            {
                setSpace(1, 3, 0, box1);
            }
            else
            {
                setSpace(1, 3, 1, box1);
            }

            checkForOverTurns(1, 3);
            switchTurn();
        }

        private void Box27_Click(object sender, EventArgs e)
        {
            if (this.board[2, 3] != -1)
                return;

            if (black_turn)
            {
                setSpace(2, 3, 0, box1);
            }
            else
            {
                setSpace(2, 3, 1, box1);
            }

            checkForOverTurns(2, 3);
            switchTurn();
        }

        private void Box28_Click(object sender, EventArgs e)
        {
            if (this.board[3, 3] != -1)
                return;

            if (black_turn)
            {
                setSpace(3, 3, 0, box1);
            }
            else
            {
                setSpace(3, 3, 1, box1);
            }

            checkForOverTurns(3, 3);
            switchTurn();
        }

        private void Box29_Click(object sender, EventArgs e)
        {
            if (this.board[4, 3] != -1)
                return;

            if (black_turn)
            {
                setSpace(4, 3, 0, box1);
            }
            else
            {
                setSpace(4, 3, 1, box1);
            }

            checkForOverTurns(4, 3);
            switchTurn();
        }

        private void Box30_Click(object sender, EventArgs e)
        {
            if (this.board[5, 3] != -1)
                return;

            if (black_turn)
            {
                setSpace(5, 3, 0, box1);
            }
            else
            {
                setSpace(5, 3, 1, box1);
            }

            checkForOverTurns(5, 3);
            switchTurn();
        }

        private void Box31_Click(object sender, EventArgs e)
        {
            if (this.board[6, 3] != -1)
                return;

            if (black_turn)
            {
                setSpace(6, 3, 0, box1);
            }
            else
            {
                setSpace(6, 3, 1, box1);
            }

            checkForOverTurns(6, 3);
            switchTurn();
        }

        private void Box32_Click(object sender, EventArgs e)
        {
            if (this.board[7, 3] != -1)
                return;

            if (black_turn)
            {
                setSpace(7, 3, 0, box1);
            }
            else
            {
                setSpace(7, 3, 1, box1);
            }

            checkForOverTurns(7, 3);
            switchTurn();
        }

        /*
        * 5th Row
        */

        private void Box33_Click(object sender, EventArgs e)
        {
            if (this.board[0, 4] != -1)
                return;

            if (black_turn)
            {
                setSpace(0, 4, 0, box1);
            }
            else
            {
                setSpace(0, 4, 1, box1);
            }

            checkForOverTurns(0, 4);
            switchTurn();
        }

        private void Box34_Click(object sender, EventArgs e)
        {
            if (this.board[1, 4] != -1)
                return;

            if (black_turn)
            {
                setSpace(1, 4, 0, box1);
            }
            else
            {
                setSpace(1, 4, 1, box1);
            }

            checkForOverTurns(1, 4);
            switchTurn();
        }

        private void Box35_Click(object sender, EventArgs e)
        {
            if (this.board[2, 4] != -1)
                return;

            if (black_turn)
            {
                setSpace(2, 4, 0, box1);
            }
            else
            {
                setSpace(2, 4, 1, box1);
            }

            checkForOverTurns(2, 4);
            switchTurn();
        }

        private void Box36_Click(object sender, EventArgs e)
        {
            if (this.board[3, 4] != -1)
                return;

            if (black_turn)
            {
                setSpace(3, 4, 0, box1);
            }
            else
            {
                setSpace(3, 4, 1, box1);
            }

            checkForOverTurns(3, 4);
            switchTurn();
        }

        private void Box37_Click(object sender, EventArgs e)
        {
            if (this.board[4, 4] != -1)
                return;

            if (black_turn)
            {
                setSpace(4, 4, 0, box1);
            }
            else
            {
                setSpace(4, 4, 1, box1);
            }

            checkForOverTurns(4, 4);
            switchTurn();
        }

        private void Box38_Click(object sender, EventArgs e)
        {
            if (this.board[5, 4] != -1)
                return;

            if (black_turn)
            {
                setSpace(5, 4, 0, box1);
            }
            else
            {
                setSpace(5, 4, 1, box1);
            }

            checkForOverTurns(5, 4);
            switchTurn();
        }

        private void Box39_Click(object sender, EventArgs e)
        {
            if (this.board[6, 4] != -1)
                return;

            if (black_turn)
            {
                setSpace(6, 4, 0, box1);
            }
            else
            {
                setSpace(6, 4, 1, box1);
            }

            checkForOverTurns(6, 4);
            switchTurn();
        }

        private void Box40_Click(object sender, EventArgs e)
        {
            if (this.board[7, 4] != -1)
                return;

            if (black_turn)
            {
                setSpace(7, 4, 0, box1);
            }
            else
            {
                setSpace(7, 4, 1, box1);
            }

            checkForOverTurns(7, 4);
            switchTurn();
        }

        /*
         * 6th Row Listeners 
         */

        private void Box41_Click(object sender, EventArgs e)
        {
            if (this.board[0, 5] != -1)
                return;

            if (black_turn)
            {
                setSpace(0, 5, 0, box1);
            }
            else
            {
                setSpace(0, 5, 1, box1);
            }

            checkForOverTurns(0, 5);
            switchTurn();
        }

        private void Box42_Click(object sender, EventArgs e)
        {
            if (this.board[1, 5] != -1)
                return;

            if (black_turn)
            {
                setSpace(1, 5, 0, box1);
            }
            else
            {
                setSpace(1, 5, 1, box1);
            }

            checkForOverTurns(1, 5);
            switchTurn();
        }

        private void Box43_Click(object sender, EventArgs e)
        {
            if (this.board[2, 5] != -1)
                return;

            if (black_turn)
            {
                setSpace(2, 5, 0, box1);
            }
            else
            {
                setSpace(2, 5, 1, box1);
            }

            checkForOverTurns(2, 5);
            switchTurn();
        }

        private void Box44_Click(object sender, EventArgs e)
        {
            if (this.board[3, 5] != -1)
                return;

            if (black_turn)
            {
                setSpace(3, 5, 0, box1);
            }
            else
            {
                setSpace(3, 5, 1, box1);
            }

            checkForOverTurns(3, 5);
            switchTurn();
        }

        private void Box45_Click(object sender, EventArgs e)
        {
            if (this.board[4, 5] != -1)
                return;

            if (black_turn)
            {
                setSpace(4, 5, 0, box1);
            }
            else
            {
                setSpace(4, 5, 1, box1);
            }

            checkForOverTurns(4, 5);
            switchTurn();
        }

        private void Box46_Click(object sender, EventArgs e)
        {
            if (this.board[5, 5] != -1)
                return;

            if (black_turn)
            {
                setSpace(5, 5, 0, box1);
            }
            else
            {
                setSpace(5, 5, 1, box1);
            }

            checkForOverTurns(5, 5);
            switchTurn();
        }

        private void Box47_Click(object sender, EventArgs e)
        {
            if (this.board[6, 5] != -1)
                return;

            if (black_turn)
            {
                setSpace(6, 5, 0, box1);
            }
            else
            {
                setSpace(6, 5, 1, box1);
            }

            checkForOverTurns(6, 5);
            switchTurn();
        }

        private void Box48_Click(object sender, EventArgs e)
        {
            if (this.board[7, 5] != -1)
                return;

            if (black_turn)
            {
                setSpace(7, 5, 0, box1);
            }
            else
            {
                setSpace(7, 5, 1, box1);
            }

            checkForOverTurns(7, 5);
            switchTurn();
        }

        /*
         * 7th Row Listeners
         */

        private void Box49_Click(object sender, EventArgs e)
        {
            if (this.board[0, 6] != -1)
                return;

            if (black_turn)
            {
                setSpace(0, 6, 0, box1);
            }
            else
            {
                setSpace(0, 6, 1, box1);
            }

            checkForOverTurns(0, 6);
            switchTurn();
        }

        private void Box50_Click(object sender, EventArgs e)
        {
            if (this.board[1, 6] != -1)
                return;

            if (black_turn)
            {
                setSpace(1, 6, 0, box1);
            }
            else
            {
                setSpace(1, 6, 1, box1);
            }

            checkForOverTurns(1, 6);
            switchTurn();
        }

        private void Box51_Click(object sender, EventArgs e)
        {
            if (this.board[2, 6] != -1)
                return;

            if (black_turn)
            {
                setSpace(2, 6, 0, box1);
            }
            else
            {
                setSpace(2, 6, 1, box1);
            }

            checkForOverTurns(2, 6);
            switchTurn();
        }

        private void Box52_Click(object sender, EventArgs e)
        {
            if (this.board[3, 6] != -1)
                return;

            if (black_turn)
            {
                setSpace(3, 6, 0, box1);
            }
            else
            {
                setSpace(3, 6, 1, box1);
            }

            checkForOverTurns(3, 6);
            switchTurn();
        }

        private void Box53_Click(object sender, EventArgs e)
        {
            if (this.board[4, 6] != -1)
                return;

            if (black_turn)
            {
                setSpace(4, 6, 0, box1);
            }
            else
            {
                setSpace(4, 6, 1, box1);
            }

            checkForOverTurns(4, 6);
            switchTurn();
        }

        private void Box54_Click(object sender, EventArgs e)
        {
            if (this.board[5, 6] != -1)
                return;

            if (black_turn)
            {
                setSpace(5, 6, 0, box1);
            }
            else
            {
                setSpace(5, 6, 1, box1);
            }

            checkForOverTurns(5, 6);
            switchTurn();
        }

        private void Box55_Click(object sender, EventArgs e)
        {
            if (this.board[6, 6] != -1)
                return;

            if (black_turn)
            {
                setSpace(6, 6, 0, box1);
            }
            else
            {
                setSpace(6, 6, 1, box1);
            }

            checkForOverTurns(6, 6);
            switchTurn();
        }

        private void Box56_Click(object sender, EventArgs e)
        {
            if (this.board[7, 6] != -1)
                return;

            if (black_turn)
            {
                setSpace(7, 6, 0, box1);
            }
            else
            {
                setSpace(7, 6, 1, box1);
            }

            checkForOverTurns(7, 6);
            switchTurn();
        }

        /*
         * 8th Row Listeners
         */

        private void Box57_Click(object sender, EventArgs e)
        {
            if (this.board[0, 7] != -1)
                return;

            if (black_turn)
            {
                setSpace(0, 7, 0, box1);
            }
            else
            {
                setSpace(0, 7, 1, box1);
            }

            checkForOverTurns(0, 7);
            switchTurn();
        }

        private void Box58_Click(object sender, EventArgs e)
        {
            if (this.board[1, 7] != -1)
                return;

            if (black_turn)
            {
                setSpace(1, 7, 0, box1);
            }
            else
            {
                setSpace(1, 7, 1, box1);
            }

            checkForOverTurns(1, 7);
            switchTurn();
        }

        private void Box59_Click(object sender, EventArgs e)
        {
            if (this.board[2, 7] != -1)
                return;

            if (black_turn)
            {
                setSpace(2, 7, 0, box1);
            }
            else
            {
                setSpace(2, 7, 1, box1);
            }

            checkForOverTurns(2, 7);
            switchTurn();
        }

        private void Box60_Click(object sender, EventArgs e)
        {
            if (this.board[3, 7] != -1)
                return;

            if (black_turn)
            {
                setSpace(3, 7, 0, box1);
            }
            else
            {
                setSpace(3, 7, 1, box1);
            }

            checkForOverTurns(3, 7);
            switchTurn();
        }

        private void Box61_Click(object sender, EventArgs e)
        {
            if (this.board[4, 7] != -1)
                return;

            if (black_turn)
            {
                setSpace(4, 7, 0, box1);
            }
            else
            {
                setSpace(4, 7, 1, box1);
            }

            checkForOverTurns(4, 7);
            switchTurn();
        }

        private void Box62_Click(object sender, EventArgs e)
        {
            if (this.board[5, 7] != -1)
                return;

            if (black_turn)
            {
                setSpace(5, 7, 0, box1);
            }
            else
            {
                setSpace(5, 7, 1, box1);
            }

            checkForOverTurns(5, 7);
            switchTurn();
        }

        private void Box63_Click(object sender, EventArgs e)
        {
            if (this.board[6, 7] != -1)
                return;

            if (black_turn)
            {
                setSpace(6, 7, 0, box1);
            }
            else
            {
                setSpace(6, 7, 1, box1);
            }

            checkForOverTurns(6, 7);
            switchTurn();
        }

        private void Box64_Click(object sender, EventArgs e)
        {
            if (this.board[7, 7] != -1)
                return;

            if (black_turn)
            {
                setSpace(7, 7, 0, box1);
            }
            else
            {
                setSpace(7, 7, 1, box1);
            }

            checkForOverTurns(7, 7);
            switchTurn();
        }

        //Returns the PictureBox for the corisponding row / column.
        private PictureBox getBoxFromRowAndColum(int row, int colum)
        {
            switch (row)
            {
                /*
                 * First Row
                 */
                case 0 when colum == 0:
                    {
                        return box1;
                    }
                    break;
                case 1 when colum == 0:
                    {
                        return box2;
                    }
                    break;
                case 2 when colum == 0:
                    {
                        return box3;
                    }
                    break;
                case 3 when colum == 0:
                    {
                        return box4;
                    }
                    break;
                case 4 when colum == 0:
                    {
                        return box5;
                    }
                    break;
                case 5 when colum == 0:
                    {
                        return box6;
                    }
                    break;
                case 6 when colum == 0:
                    {
                        return box7;
                    }
                    break;
                case 7 when colum == 0:
                    {
                        return box8;
                    }
                    break;

                /*
                 * Seccond Row
                 */

                case 0 when colum == 1:
                    {
                        return box9;
                    }
                    break;
                case 1 when colum == 1:
                    {
                        return box10;
                    }
                    break;
                case 2 when colum == 1:
                    {
                        return box11;
                    }
                    break;
                case 3 when colum == 1:
                    {
                        return box12;
                    }
                    break;
                case 4 when colum == 1:
                    {
                        return box13;
                    }
                    break;
                case 5 when colum == 1:
                    {
                        return box14;
                    }
                    break;
                case 6 when colum == 1:
                    {
                        return box15;
                    }
                    break;
                case 7 when colum == 1:
                    {
                        return box16;
                    }
                    break;

                /*
                * Third Row
                */

                case 0 when colum == 2:
                    {
                        return box17;
                    }
                    break;
                case 1 when colum == 2:
                    {
                        return box18;
                    }
                    break;
                case 2 when colum == 2:
                    {
                        return box19;
                    }
                    break;
                case 3 when colum == 2:
                    {
                        return box20;
                    }
                    break;
                case 4 when colum == 2:
                    {
                        return box21;
                    }
                    break;
                case 5 when colum == 2:
                    {
                        return box22;
                    }
                    break;
                case 6 when colum == 2:
                    {
                        return box23;
                    }
                    break;
                case 7 when colum == 2:
                    {
                        return box24;
                    }
                    break;

                /*
                * Fourth Row
                */

                case 0 when colum == 3:
                    {
                        return box25;
                    }
                    break;
                case 1 when colum == 3:
                    {
                        return box26;
                    }
                    break;
                case 2 when colum == 3:
                    {
                        return box27;
                    }
                    break;
                case 3 when colum == 3:
                    {
                        return box28;
                    }
                    break;
                case 4 when colum == 3:
                    {
                        return box29;
                    }
                    break;
                case 5 when colum == 3:
                    {
                        return box30;
                    }
                    break;
                case 6 when colum == 3:
                    {
                        return box31;
                    }
                    break;
                case 7 when colum == 3:
                    {
                        return box32;
                    }
                    break;

                /*
                * Fifth Row
                */

                case 0 when colum == 4:
                    {
                        return box33;
                    }
                    break;
                case 1 when colum == 4:
                    {
                        return box34;
                    }
                    break;
                case 2 when colum == 4:
                    {
                        return box35;
                    }
                    break;
                case 3 when colum == 4:
                    {
                        return box36;
                    }
                    break;
                case 4 when colum == 4:
                    {
                        return box37;
                    }
                    break;
                case 5 when colum == 4:
                    {
                        return box38;
                    }
                    break;
                case 6 when colum == 4:
                    {
                        return box39;
                    }
                    break;
                case 7 when colum == 4:
                    {
                        return box40;
                    }
                    break;

                /*
                * Sixth Row
                */

                case 0 when colum == 5:
                    {
                        return box41;
                    }
                    break;
                case 1 when colum == 5:
                    {
                        return box42;
                    }
                    break;
                case 2 when colum == 5:
                    {
                        return box43;
                    }
                    break;
                case 3 when colum == 5:
                    {
                        return box44;
                    }
                    break;
                case 4 when colum == 5:
                    {
                        return box45;
                    }
                    break;
                case 5 when colum == 5:
                    {
                        return box46;
                    }
                    break;
                case 6 when colum == 5:
                    {
                        return box47;
                    }
                    break;
                case 7 when colum == 5:
                    {
                        return box48;
                    }
                    break;

                /*
                * Seventh Row
                */

                case 0 when colum == 6:
                    {
                        return box49;
                    }
                    break;
                case 1 when colum == 6:
                    {
                        return box50;
                    }
                    break;
                case 2 when colum == 6:
                    {
                        return box51;
                    }
                    break;
                case 3 when colum == 6:
                    {
                        return box52;
                    }
                    break;
                case 4 when colum == 6:
                    {
                        return box53;
                    }
                    break;
                case 5 when colum == 6:
                    {
                        return box54;
                    }
                    break;
                case 6 when colum == 6:
                    {
                        return box55;
                    }
                    break;
                case 7 when colum == 6:
                    {
                        return box56;
                    }
                    break;


                /*
                * Eigth Row
                */

                case 0 when colum == 7:
                    {
                        return box57;
                    }
                    break;
                case 1 when colum == 7:
                    {
                        return box58;
                    }
                    break;
                case 2 when colum == 7:
                    {
                        return box59;
                    }
                    break;
                case 3 when colum == 7:
                    {
                        return box60;
                    }
                    break;
                case 4 when colum == 7:
                    {
                        return box61;
                    }
                    break;
                case 5 when colum == 7:
                    {
                        return box62;
                    }
                    break;
                case 6 when colum == 7:
                    {
                        return box63;
                    }
                    break;
                case 7 when colum == 7:
                    {
                        return box64;
                    }
                    break;
            }

            return null;
        }

        //Unused Class

        public class Boardspace
        {
            private int row;
            private int colum;
            private bool isfirstplaced;

            public Boardspace(int row, int colum, bool isfirstplaced)
            {
                this.row = row;
                this.colum = colum;
                this.isfirstplaced = isfirstplaced;
            }

            public int getRow()
            {
                return row;
            }

            public int getColum()
            {
                return colum;
            }

            private bool isFirstPlaced()
            {
                return isfirstplaced;
            }
        }


    }
}