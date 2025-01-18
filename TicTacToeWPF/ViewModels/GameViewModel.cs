using System.Collections.ObjectModel;
using TicTacToeWPF.Commands;
using TicTacToeWPF.Models;

namespace TicTacToeWPF.ViewModels
{
    public class GameViewModel : BaseViewModel
    {

        private ObservableCollection<CellViewModel> board = new();

        public ObservableCollection<CellViewModel> Board
        {
            get { return board; }
            set
            {
                board = value;
                OnPropertyChanged();
            }
        }


        public DelegateCommand NewGameCommand { get; }
        public DelegateCommand OnCellClickCommand { get; }

        public GameViewModel()
        {
            NewGameCommand = new DelegateCommand(_ => NewGame());
            OnCellClickCommand = new DelegateCommand(OnCellClick);

            NewGame();
        }

        private void NewGame()
        {
            Board = new ObservableCollection<CellViewModel>();
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    Cell cell = new Cell { X = x, Y = y };
                    Board.Add(new CellViewModel(cell));
                }
            }
        }

        private void OnCellClick(object parameter)
        {
            if (parameter is CellViewModel cellViewModel && cellViewModel.State == CellState.Empty)
            {
                cellViewModel.State = CellState.Player;

                if (CheckWinningCondition())
                {
                    return;
                }


                AIMove();
            }
        }

        private void AIMove()
        {
            CalculatedMove();

            CheckWinningCondition();

            return;
        }

        private bool CheckWinningCondition()
        {
            if(Board.All(c => c.State != CellState.Empty))
            {
                HandleOutcome(CellState.Empty);
                return true;
            }

            CellsMatching(Board[0], Board[1], Board[2]);
            CellsMatching(Board[3], Board[4], Board[5]);
            CellsMatching(Board[6], Board[7], Board[8]);
            CellsMatching(Board[0], Board[3], Board[6]);
            CellsMatching(Board[1], Board[4], Board[7]);
            CellsMatching(Board[2], Board[5], Board[8]);
            CellsMatching(Board[0], Board[4], Board[8]);
            CellsMatching(Board[2], Board[4], Board[6]);

            return false;
        }

        private void CellsMatching(CellViewModel a, CellViewModel b, CellViewModel c)
        {
            if( a.State != CellState.Empty && a.State == b.State && b.State == c.State)
            {
                HandleOutcome(a.State);
            }
        }

        private void HandleOutcome(CellState state)
        {
            if (state == CellState.Player)
            {
                System.Windows.MessageBox.Show($"You Wins!", "Game Over"); //this is should not be possible
            }
            if (state == CellState.Computer)
            {
                System.Windows.MessageBox.Show($"You Lose!", "Game Over");
            }
            else
            {
                System.Windows.MessageBox.Show($"It's a Draw!", "Game Over");
            }

            NewGame();
        }

        private void CalculatedMove()
        {
            if(TakeOpportunity(Board[0], Board[1], Board[2]) ||
               TakeOpportunity(Board[3], Board[4], Board[5]) ||
               TakeOpportunity(Board[6], Board[7], Board[8]) ||
               TakeOpportunity(Board[0], Board[3], Board[6]) ||
               TakeOpportunity(Board[1], Board[4], Board[7]) ||
               TakeOpportunity(Board[2], Board[5], Board[8]) ||
               TakeOpportunity(Board[0], Board[4], Board[8]) ||
               TakeOpportunity(Board[2], Board[4], Board[6]))
            {
                return;
            }

            if (StopOpportunity(Board[0], Board[1], Board[2]) ||
                StopOpportunity(Board[3], Board[4], Board[5]) ||
                StopOpportunity(Board[6], Board[7], Board[8]) ||
                StopOpportunity(Board[0], Board[3], Board[6]) ||
                StopOpportunity(Board[1], Board[4], Board[7]) ||
                StopOpportunity(Board[2], Board[5], Board[8]) ||
                StopOpportunity(Board[0], Board[4], Board[8]) ||
                StopOpportunity(Board[2], Board[4], Board[6]))
            {
                return;
            }

            if (Board[4].State == CellState.Empty)
            {
                Board[4].State = CellState.Computer;
                return;
            }

            if (TakeCorner())
            {
                return;
            }

            foreach (var cell in Board)
            {
                if (cell.State == CellState.Empty)
                {
                    cell.State = CellState.Computer;
                    break;
                }
            }

        }

        private bool TakeOpportunity(CellViewModel a, CellViewModel b, CellViewModel c)
        {
            if (a.State == CellState.Empty)
            {
                if (b.State == CellState.Computer && c.State == CellState.Computer)
                {
                    a.State = CellState.Computer;
                    return true;
                }
            }
            if (b.State == CellState.Empty)
            {
                if (a.State == CellState.Computer && c.State == CellState.Computer)
                {
                    b.State = CellState.Computer;
                    return true;
                }
            }
            if (c.State == CellState.Empty)
            {
                if (a.State == CellState.Computer && b.State == CellState.Computer)
                {
                    c.State = CellState.Computer;
                    return true;
                }
            }
            return false;
        }

        private bool StopOpportunity(CellViewModel a, CellViewModel b, CellViewModel c)
        {
            if (a.State == CellState.Empty)
            {
                if (b.State == CellState.Player && c.State == CellState.Player)
                {
                    a.State = CellState.Computer;
                    return true;
                }
            }
            if (b.State == CellState.Empty)
            {
                if (a.State == CellState.Player && c.State == CellState.Player)
                {
                    b.State = CellState.Computer;
                    return true;
                }
            }
            if (c.State == CellState.Empty)
            {
                if (a.State == CellState.Player && b.State == CellState.Player)
                {
                    c.State = CellState.Computer;
                    return true;
                }
            }
            return false;
        }

        private bool TakeCorner()
        {
            //check corner 0
            if (Board[0].State == CellState.Empty)
            {
                if (Board[1].State == CellState.Player || Board[2].State == CellState.Player)
                {
                    if (Board[3].State == CellState.Player || Board[4].State == CellState.Player)
                    {
                        Board[0].State = CellState.Computer;
                        return true;
                    }
                }
            }

            //check corner 2
            if (Board[2].State == CellState.Empty)
            {
                if (Board[0].State == CellState.Player || Board[1].State == CellState.Player)
                {
                    if (Board[5].State == CellState.Player || Board[8].State == CellState.Player)
                    {
                        Board[2].State = CellState.Computer;
                        return true;
                    }
                }
            }

            //check corner 6
            if (Board[6].State == CellState.Empty)
            {
                if (Board[0].State == CellState.Player || Board[3].State == CellState.Player)
                {
                    if (Board[7].State == CellState.Player || Board[8].State == CellState.Player)
                    {
                        Board[6].State = CellState.Computer;
                        return true;
                    }
                }
            }

            //check corner 8
            if (Board[8].State == CellState.Empty)
            {
                if (Board[2].State == CellState.Player || Board[5].State == CellState.Player)
                {
                    if (Board[6].State == CellState.Player || Board[7].State == CellState.Player)
                    {
                        Board[8].State = CellState.Computer;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
