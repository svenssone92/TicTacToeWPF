using System.Windows.Media;
using TicTacToeWPF.Models;

namespace TicTacToeWPF.ViewModels
{
    public class CellViewModel : BaseViewModel
    {
        private readonly Cell _cell;

        private Brush _background;

        public CellViewModel(Cell cell)
        {
            this._cell = cell;
            _background = SetColor("#b8b8b8");
        }

        public int X
        {
            get { return _cell.X; }
            set
            {
                _cell.X = value;
                OnPropertyChanged();
            }
        }

        public int Y
        {
            get { return _cell.Y; }
            set
            {
                _cell.Y = value;
                OnPropertyChanged();
            }
        }

        public CellState State
        {
            get { return _cell.State; }
            set
            {
                _cell.State = value;

                UpdateBackground();
                OnPropertyChanged();
            }
        }

        public Brush Background
        {
            get { return _background; }
            set
            {
                _background = value;
                OnPropertyChanged();
            }
        }

        private void UpdateBackground()
        {
            Background = State switch
            {
                CellState.Player => SetColor("#31ade6"),
                CellState.Computer => SetColor("#e66a31"),
                CellState.Empty => SetColor("#b8b8b8"),
                _ => Brushes.Transparent
            };
        }

        private Brush SetColor(string hexString)
        {
            try
            {
                return (Brush?)new BrushConverter().ConvertFromString(hexString) ?? Brushes.Black;
            }
            catch
            {
                return Brushes.Black;
            }
        }
    }
}
