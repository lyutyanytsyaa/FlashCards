using System.Drawing;
using System.Windows.Forms;

namespace FlashcardsApp
{
    partial class StudyForm
    {
        Label lblQ,lblA;
        Button show,know,so,dont;

        void InitializeComponent()
        {
            lblQ=new(){Font=new Font("Segoe UI",14),Bounds=new(20,20,460,40)};
            lblA=new(){Font=new Font("Segoe UI",12),Bounds=new(20,70,460,40)};
            show=new(){Text="Показати",Location=new(20,130)};
            dont=new(){Text="Не знаю",Location=new(120,130),BackColor = Color.FromArgb(60, 0, 0),ForeColor = Color.Red, };
            so=new(){Text="Так собі",Location=new(220,130),BackColor = Color.LightGreen,ForeColor = Color.Black};
            know=new(){Text="Знаю",Location=new(320,130), BackColor = Color.FromArgb(0, 0, 40),ForeColor = Color.DeepSkyBlue};
            
            show.Click+=this.show_Click;
            dont.Click+=this.dont_Click;
            so.Click+=this.so_Click;
            know.Click+=this.know_Click;

            Controls.AddRange(new Control[]{lblQ,lblA,show,know,so,dont});
            ClientSize=new(500,200);
            Text="Вчити";
        }
    }
}