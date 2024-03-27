using ClosedXML.Excel;
using SQLAnywhere;
using System.Globalization;

namespace comparacao_pis_cofins
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            var resposta = fbd.ShowDialog();
            if (resposta == DialogResult.OK)
            {
                txtCaminhoSped.Text = fbd.SelectedPath;
            }
        }

        private void btnComparar_Click(object sender, EventArgs e)
        {
            if (txtCaminhoSped.Text == "") {
                MessageBox.Show("Você não escolheu a pasta cara, por quê você é assim cara? \nTive o maior trabalho escolhe o caminho aí cara.", "Erro de preenchimento!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } if (!Directory.Exists(txtCaminhoSped.Text))
            {
                MessageBox.Show("Você é trouxa demais, preenche o caminho direito cara!", "Erro de preenchimento!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } if (txtCaminhoPlanilha.Text == "")
            {
                MessageBox.Show("E aí cara, escolhe o caminho para salvar a planilha aí, ta querendo matar meu processo? Qual que é a tua?", "Erro de preenchimento!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;            
            }
            var arquivos = Directory.GetFiles(txtCaminhoSped.Text, "*.txt", SearchOption.AllDirectories);
            Dictionary<DateTime, List<Comparacao>> dicionario = new Dictionary<DateTime, List<Comparacao>>();
            foreach (string caminho_arquivo in arquivos)
            {
                Comparacao dados = new Comparacao();
                var conteudo = File.ReadAllLines(caminho_arquivo);
                string primeiraLinha = conteudo[0];
                string[] dadosPrimeiraLinha = primeiraLinha.Split('|');
                dados.Cnpj = dadosPrimeiraLinha[9];
                dados.RazaoSocial = dadosPrimeiraLinha[8];
                dados.Periodo = DateTime.ParseExact(dadosPrimeiraLinha[6], "ddMMyyyy", CultureInfo.InvariantCulture);

                string linhaM200 = conteudo.FirstOrDefault(x => x.StartsWith("|M200|"));
                string[] dadosLinhaM200 = linhaM200.Split('|');
                dados.ValorPisEFD = double.Parse(dadosLinhaM200[13]);

                string linhaM600 = conteudo.FirstOrDefault(x => x.StartsWith("|M600|"));
                string[] dadosLinhaM600 = linhaM600.Split('|');
                dados.ValorCofinsEFD = double.Parse(dadosLinhaM600[13]);
                Conexao.TestarConexao();

                dados.CodEmpresa = Conexao.ObterDadoUnitario<int>($"SELECT codi_emp FROM bethadba.geempre WHERE cgce_emp = '{dados.Cnpj}'");
                string query = File.ReadAllText("query.sql");
                query = query.Replace("#data", dados.Periodo.ToString("yyyy-MM-dd"));
                query = query.Replace("#codEmpresa", dados.CodEmpresa.ToString());
                dados.ValorPisDominio = Conexao.ObterDadoUnitario<double>(query.Replace("#codImposto", "17"));
                dados.ValorCofinsDominio = Conexao.ObterDadoUnitario<double>(query.Replace("#codImposto", "19"));

                if (!dicionario.ContainsKey(dados.Periodo))
                {
                    dicionario.Add(dados.Periodo, new List<Comparacao>());
                }

                dicionario[dados.Periodo].Add(dados);
            }

            using(var workbook = new XLWorkbook())
            {
                foreach (var periodo in dicionario.Keys)
                {

                    var worksheet = workbook.Worksheets.Add(periodo.ToString("MM-yyyy"));
                    worksheet.Cell("A1").Value = "Cód Empresa";
                    worksheet.Column(1).Width = 11;
                    worksheet.Cell("B1").Value = "Razão Social";
                    worksheet.Column(2).Width = 50;
                    worksheet.Cell("C1").Value = "CNPJ";
                    worksheet.Column(3).Width = 17;
                    worksheet.Cell("D1").Value = "Valor PIS EFD";
                    worksheet.Column(4).Width = 11;
                    worksheet.Cell("E1").Value = "Valor COFINS EFD";
                    worksheet.Column(5).Width = 15;
                    worksheet.Cell("F1").Value = "Valor PIS Domínio";
                    worksheet.Column(6).Width = 15;
                    worksheet.Cell("G1").Value = "Valor COFINS Dominío";
                    worksheet.Column(7).Width = 19;
                    worksheet.Range(1, 3, 1, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    // teste

                    int linha = 2;
                    foreach (var comparacao in dicionario[periodo]) {
                        worksheet.Cell(linha, 1).Value = comparacao.CodEmpresa;
                        worksheet.Cell(linha, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cell(linha, 2).Value = comparacao.RazaoSocial;
                        worksheet.Cell(linha, 3).Value = comparacao.Cnpj;
                        worksheet.Cell(linha, 3).Style.NumberFormat.Format = "00\".\"000\".\"000\"/\"0000\"-\"00";
                        worksheet.Cell(linha, 4).Value = comparacao.ValorPisEFD;
                        worksheet.Cell(linha, 5).Value = comparacao.ValorCofinsEFD;
                        worksheet.Cell(linha, 6).Value = comparacao.ValorPisDominio;
                        worksheet.Cell(linha, 7).Value = comparacao.ValorCofinsDominio;
                        worksheet.Range(linha, 4, linha, 7).Style.NumberFormat.Format = "R$ 0.00";
                        worksheet.Range(linha, 4, linha, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        linha++;
                    }

                }

                workbook.SaveAs(txtCaminhoPlanilha.Text);
            }

            MessageBox.Show("Seu arquivo foi gerado com sucesso!", "Concluido", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog salvarComo = new SaveFileDialog();
            salvarComo.Filter = "Planilha do Excel |*.xlsx";
            salvarComo.FileName = "Comparação";
            var resposta = salvarComo.ShowDialog();
            if (resposta == DialogResult.OK)
            {
                txtCaminhoPlanilha.Text = salvarComo.FileName;
            } else
            {
                MessageBox.Show("De novo no mesmo erro cara, para com isso \nPreenche o caminho aí cara.", "Erro de preenchimento!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
