using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadDemo
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        internal List<Produto> Produtos { get; set; }


        //}
        private async Task TarefaLonga(string msg)
        {
            try
            {
                await ExecutarLoop();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }
        }
        public async Task ExecutarLoop()
        {
            int[] conjunto = new int[5];
            for (int c = 0; c <= 5; c++)
                conjunto[c] = c;
        }



        private async void Form1_Load(object sender, EventArgs e)
        {
            CancellationTokenSource cancelSource =
                new CancellationTokenSource();

            var token = cancelSource.Token;


            Stopwatch watch = new Stopwatch();
            watch.Start();

            var Task1 = Task.Run(async () =>
            {
                await Task.Delay(5000, token);
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                    return null;
                }
                return Produtos = new List<Produto>
                {
                    new Produto{ID=1,Nome="Tomate",Preco=33.40},
                    new Produto{ID=2,Nome="Cebola",Preco=14.40},
                    new Produto{ID=3,Nome="Mamão",Preco=5.40},
                    new Produto{ID=4,Nome="Coxinha",Preco=8.99}
                };


            }, token);
            var Task2 = Task.Run(() =>
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                    return new List<Produto>();
                }
                return Produtos = new List<Produto>
                {
                    new Produto{ID=1,Nome="Tomate",Preco=33.40},
                    new Produto{ID=2,Nome="Cebola",Preco=14.40},
                    new Produto{ID=3,Nome="Mamão",Preco=5.40},
                    new Produto{ID=4,Nome="Coxinha",Preco=8.99}
                };

            }, token);
            var Task3 = Task.Run(async () =>
            {
                await Task.Delay(4000, token);
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                    return new List<Produto>();
                }
                return Produtos = new List<Produto>
                {
                    new Produto{ID=1,Nome="Tomate",Preco=33.40},
                    new Produto{ID=2,Nome="Cebola",Preco=14.40},
                    new Produto{ID=3,Nome="Mamão",Preco=5.40},
                    new Produto{ID=4,Nome="Coxinha",Preco=8.99}
                };


            }, token);

            var taskterminouprimeiro = await Task.WhenAny(Task1, Task2, Task3);
            watch.Stop();
            cancelSource.Cancel();
            MessageBox.Show($"Tarefa levou um tempo total de:{watch.Elapsed}");

            dataGridView1.DataSource = taskterminouprimeiro.Result;
        }
    }
}
