using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PessoaTI14T
{
    public partial class Form1 : Form
    {
        DAOPessoa pessoa;
        public Form1()
        {
            InitializeComponent();
            pessoa = new DAOPessoa();//Abrindo a conexão com o Banco de Dados
            textBox1.Text = Convert.ToString( pessoa.ConsultarCodigo() + 1);//mostra o proximo codigo na tela depois do ultimo codigo cadastrado, por isso o +1
            textBox1.ReadOnly = true;//bloqueando o codigo no primeiro acesso
        }//Fim do método construtor

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void Limpar()
        {
            textBox1.Text = "" + pessoa.ConsultarCodigo();//codigo
            maskedTextBox1.Text = "";//cpf
            textBox2.Text = "";//nome
            maskedTextBox2.Text = "";//telefone
            textBox4.Text = "";//endereço

        }//fim do metodo Limpar tela

        public void AtivarCampos()
        {
                textBox1.ReadOnly = false;//codigo
                maskedTextBox1.ReadOnly = true;//cpf
                textBox2.ReadOnly = true;//nome
                maskedTextBox2.ReadOnly = true;//telefone
                textBox4.ReadOnly = true;//endereço
        }//fim do metodo ativar campos

        public void InativarCampos()
        {
            textBox1.ReadOnly = true;//codigo
            maskedTextBox1.ReadOnly = false;//cpf
            textBox2.ReadOnly = false;//nome
            maskedTextBox2.ReadOnly = false;//telefone
            textBox4.ReadOnly = false;//endreço
        }//fim do metodo inativar campos

        private void Cadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                if(textBox1.ReadOnly == false)
                {
                    Limpar();
                    InativarCampos();
                }
                else
                {
                    long cpf = TratarCPF(maskedTextBox1.Text);//Coletando o dado do campo CPF
                    string nome = textBox2.Text;//Coletando o dado do campo nome
                    string telefone = maskedTextBox2.Text;//Coletando o dado do campo telefone
                    string endereco = textBox4.Text;//Coletando o dado do campo Endereço
                                                    //Chamar o método inserir que foi criado na classe DAOPessoa
                    pessoa.Inserir(cpf, nome, telefone, endereco);//Inserir no banco os dados do formulário
                    Limpar();//limpa os campos
                }
            }catch(Exception erro)
            {
                MessageBox.Show("" + erro);
            }
        }//fim do botão cadastrar

        public long TratarCPF(string cpf)
        {
            string tratamento = cpf.Replace(",", "");
            tratamento = tratamento.Replace("-", "");
            return Convert.ToInt64(tratamento);
        }//fim do tratar cpf

        private void Consultar_Click(object sender, EventArgs e)
        {
            if(textBox1.ReadOnly == true)
            {
                AtivarCampos();
            }
            else
            {
                maskedTextBox1.Text = "" + pessoa.ConsultarCPF(Convert.ToInt32(textBox1.Text));//preenchedo campo CPF
                textBox2.Text = pessoa.ConsultarNome(Convert.ToInt32(textBox1.Text));//preenchedo campo NOME
                maskedTextBox2.Text = pessoa.ConsultarTelefone(Convert.ToInt32(textBox1.Text));//preenchendo campo TELEFONE
                textBox4.Text = pessoa.ConsultarEndereco(Convert.ToInt32(textBox1.Text));//preenchendo campo ENDEREÇO
            }
            
        }//fim do botão consultar

        private void Atualizar_Click(object sender, EventArgs e)
        {
            //mesmos metodos do consultar, porem acrescentar if/else:
            if(textBox1.Text == "")//se textBox.Text (nome, no caso) esta vazio, entao preenche com os dados do banco
            {
                maskedTextBox1.Text = "" + pessoa.ConsultarCPF(Convert.ToInt32(textBox1.Text));//preenchedo campo CPF
                textBox2.Text = pessoa.ConsultarNome(Convert.ToInt32(textBox1.Text));//preenchedo campo NOME
                maskedTextBox2.Text = pessoa.ConsultarTelefone(Convert.ToInt32(textBox1.Text));//preenchendo campo TELEFONE
                textBox4.Text = pessoa.ConsultarEndereco(Convert.ToInt32(textBox1.Text));//preenchendo campo ENDEREÇO
            }
            else//se nao estiver vazio, atualizar com novos dados:
            {
                //apos repetir os metodos, atualiza-los:
                pessoa.Atualizar(Convert.ToInt32(textBox1.Text), "cpf", TratarCPF(maskedTextBox1.Text));//atualizar cpf
                pessoa.Atualizar(Convert.ToInt32(textBox1.Text), "nome", textBox2.Text); //atualizar nome
                pessoa.Atualizar(Convert.ToInt32(textBox1.Text), "telefone", maskedTextBox2.Text);//atualizar telefone
                pessoa.Atualizar(Convert.ToInt32(textBox1.Text), "endereco", textBox4.Text);//atualizar endereço
                Limpar();
            }
        }//fim do botão Atualizar

        private void Excluir_Click(object sender, EventArgs e)
        {
            pessoa.Deletar(Convert.ToInt32(textBox1.Text));
        }//fim do botão Excluir

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }//textbox de código

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }//maskedTextBox de CPF

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }//textBox de nome

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }//MaskedTextBox de Telefone

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }//TextBox de Endereço
    }//fim da classe
}//fim do projeto
