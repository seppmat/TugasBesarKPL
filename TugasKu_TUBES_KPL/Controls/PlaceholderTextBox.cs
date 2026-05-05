using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TugasKu_TUBES_KPL.Controls
{
    public class PlaceholderTextBox : TextBox
    {
        private string _placeholder = "";
        private bool _isPlaceholderVisible = false;
        private Color _placeholderColor = Color.Gray;

        [Category("Appearance")]
        public string Placeholder
        {
            get => _placeholder;
            set { _placeholder = value; UpdatePlaceholder(); }
        }

        [Category("Appearance")]
        public Color PlaceholderColor
        {
            get => _placeholderColor;
            set { _placeholderColor = value; if (_isPlaceholderVisible) Invalidate(); }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            UpdatePlaceholder();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            UpdatePlaceholder();
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (_isPlaceholderVisible)
            {
                Text = "";
                ForeColor = Color.Black;
                _isPlaceholderVisible = false;
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            UpdatePlaceholder();
        }

        private void UpdatePlaceholder()
        {
            if (string.IsNullOrWhiteSpace(Text) && !string.IsNullOrEmpty(_placeholder) && !Focused)
            {
                _isPlaceholderVisible = true;
                ForeColor = _placeholderColor;
                Text = _placeholder;
            }
            else if (_isPlaceholderVisible && !string.IsNullOrEmpty(Text) && Text == _placeholder)
            {
                // Do nothing, placeholder already shown
            }
            else if (!string.IsNullOrEmpty(Text) && Text != _placeholder)
            {
                _isPlaceholderVisible = false;
                ForeColor = Color.Black;
            }
        }
    }
}