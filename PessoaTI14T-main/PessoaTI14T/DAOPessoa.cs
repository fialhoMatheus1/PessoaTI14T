using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace PessoaTI14T
{
    class DAOPessoa
    {
        public MySqlConnection conexao;
        public string dados;
        public string comando;
        public string resultado;
        //declarar os vetores
        public int[] vetorCodigo;
        public long[] vetorCPF;
        public string[] vetorNome;
        public string[] vetorTelefone;
        public string[] vetorEndereco;
        public int i;//declarando o contador do for e do while
        public int contador;//declarando o contador pra contar os loops do while
        public string msg;//declarando a variavel pra guardar mensagem dos dados guardados no banco
        public int contarCodigo;//contador exclusivo para codigos

        public DAOPessoa()
        {
            conexao = new MySqlConnection("server=localhost;DataBase=turmaTI14T;Uid=root;password=");
            try
            {
                conexao.Open();//Tentando conectar ao BD
                MessageBox.Show("Conectado com Sucesso!");
            }catch(Exception erro)
            {
                MessageBox.Show("Algo deu errado!\n\n" + erro);//Enviando a mesagem de erro aos usuários
                conexao.Close();//fechando a conexão com o banco de dados
            }
        }//fim do DAOPessoa

        public void Inserir(long cpf, string nome, string telefone, string endereco)
        {
            try
            {
                //Preparar os dados para inserir no banco
                dados = "('','" + cpf + "','" + nome + "','" + telefone + "','" + endereco + "')";
                comando = "Insert into pessoaTI14T(codigo, cpf, nome, telefone, endereco) values " + dados;

                //Executar o comando na base de dados
                MySqlCommand sql = new MySqlCommand(comando, conexao);
                resultado = "" + sql.ExecuteNonQuery();
                MessageBox.Show(resultado + " Linha Afetada!");
            }catch(Exception erro)
            {
                MessageBox.Show("Algo deu errado!\n\n" + erro);
            }
        }//fim do método inserir

        public void PreencherVetor()
        {
            string query = "select * from pessoaTI14T";//comand para coletar todos os dados do banco

            //instanciando os vetores
            vetorCodigo = new int[100];
            vetorCPF = new long[100];
            vetorNome = new string[100];
            vetorTelefone = new string[100];
            vetorEndereco = new string[100];

            //preencher os vetores previamente criados, ou seja, da-los valores inicias
            for (i = 0; i < 100; i++)
            {
                vetorCodigo[i] = 0;
                vetorCPF[i] = 0;
                vetorNome[i] = "";
                vetorTelefone[i] = "";
                vetorEndereco[i] = "";
            }//fim do for

            //realizar os comandos de consulta ao banco de dados
            MySqlCommand coletar = new MySqlCommand(query, conexao);
            //ler os dados de acordo com o que esta no banco
            MySqlDataReader leitura = coletar.ExecuteReader(); //variavel 'leitura' faz uma consulta ao banco

            i = 0;//declaracao do contador 0 pro while
            contador = 0;//declaracao do contador 0 pro while
            contarCodigo = 0;//instanciando o contador para o codigo
            //preencher vetores com dados do banco de dados
            while (leitura.Read())//enquanto leitura for verdadeiro executa while
            {
                vetorCodigo[i] = Convert.ToInt32(leitura["codigo"]);
                vetorCPF[i] = Convert.ToInt64(leitura["cpf"]);
                vetorNome[i] = leitura["nome"] + "";//concateno com aspsa para converter pra string
                vetorTelefone[i] = leitura["telefone"] + "";//concateno com aspsa para converter pra string
                vetorEndereco[i] = leitura["endereco"] + "";//concateno com aspsa para converter pra string
                contarCodigo = contador;//armazenando a ultima posição do contador
                i++;//contador sai da posicao 0 e vai se incrementando
                contador++;//contar os loops do while


            }//fim do while

            leitura.Close();//fechar conexao e leitura do banco de dados


        }//fim do metodo preencher vetor

        //criar um consultar tudo por MessageBox
        public string ConsultarTudo()
        {
            PreencherVetor();//primeira coisa => preencher vetores com dados do DB

            msg = "";
            for (i = 0; i < contador; i++)
            {
                msg += "Codigo: " + vetorCodigo[i] +
                    ", CPF : " + vetorCPF[i] +
                    ", Nome; " + vetorNome[i] +
                    ", Telefone: " + vetorTelefone[i] +
                    ", Endereço: " + vetorEndereco[i] +
                    "\n\n";
            }//fim fo for
            return msg;//retorna todos os dados guardados na variavel 'msg'

        }//fim do metodo consultar tudo

        public int ConsultarCodigo()
        {
            PreencherVetor();//preencher os vetores com os dados do banco
            return vetorCodigo[contarCodigo];

        }//fim do metodo consultar codigo

        public long ConsultarCPF(int cod)
        {
            PreencherVetor();
            for(i=0; i < contador; i++)
            {
                if (vetorCodigo[i] == cod)
                {
                    return vetorCPF[i];
                }//fim do if
            }//fim do for
            return -1;
        }//fim do metodo consultar cpf

        public string ConsultarNome(int cod)
        {
            PreencherVetor();
            for (i = 0; i < contador; i++)
            {
                if(vetorCodigo[i] == cod)
                {
                    return vetorNome[i];
                }//fim du if
            }//fim do if
            return "Nome não encontrado.";
        }//fim do consultar nome

        public string ConsultarTelefone(int cod)
        {
            PreencherVetor();
            for(i = 0; i < contador; i++)
            {
                if(vetorCodigo[i] == cod)
                {
                    return vetorTelefone[i];
                }//fim do if
            }//fim do for
            return "Telefone não encontrado.";
        }//fim do consultar telefone

        public string ConsultarEndereco(int cod)
        {
            PreencherVetor();
            for (i = 0; i < contador; i++)
            {
                if (vetorCodigo[i] == cod)
                {
                    return vetorEndereco[i];
                }//fim do if
            }//fim do for
            return "Endereço não encontrado.";
        }//fim do consultar endereço

        public void Atualizar(int cod, string campo, string novoDado)
        {
            try
            {
                string query = "update pessoaTI14T set " + campo + " = '" + novoDado + "' where codigo = '" + cod + "'";

                MySqlCommand sql = new MySqlCommand(query, conexao);
                string resultado = "" + sql.ExecuteNonQuery();
                MessageBox.Show(resultado + " Linha afetada! ");
            }
            catch(Exception erro)
            {
                MessageBox.Show("" + erro);
            }
        }//fim do metodo atualizar

        //converter cpf de string para long
        public void Atualizar(int cod, string campo, long novoDado)
        {
            try
            {
                string query = "update pessoaTI14T set " + campo + " = '" + novoDado + "' where codigo = '" + cod + "'";

                MySqlCommand sql = new MySqlCommand(query, conexao);
                string resultado = "" + sql.ExecuteNonQuery();
                MessageBox.Show(resultado + " Linha afetada! ");
            }
            catch (Exception erro)
            {
                MessageBox.Show("" + erro);
            }
        }//fim do metodo atualizar

        public void Deletar(int cod)
        {
            try
            {
                string query = "delete from pessoaTI14T where codigo = '" + cod + "'";

                MySqlCommand sql = new MySqlCommand(query, conexao);
                string resultado = "" + sql.ExecuteNonQuery();
                MessageBox.Show(resultado + " Linha Afetada! ");
            }
            catch (Exception erro)
            {
                MessageBox.Show("" + erro);
            }
        }//fim do deletar
    }//fim da classe
}//fim do projeto
