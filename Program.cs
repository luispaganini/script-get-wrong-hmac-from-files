using System.Security.Cryptography;
using System.Text;

string path = "/home/luisfernando/Desktop/Criptografia/desafio_04/documentos/";
System.Console.WriteLine(GetWrongHmac(path));

static string GetWrongHmac(string path) {
    string[] nameFiles = ReadNameFiles(path);

    for(int i = 0; i < nameFiles.Length; i++)  {
        string pathWithFile = path + nameFiles[i];
        DateTime date = File.GetCreationTime(pathWithFile);
        int dateArchive = int.Parse(date.ToString("dd"));
        string hash = CalcHmac(pathWithFile, dateArchive);
        using (StreamReader sr = File.OpenText(pathWithFile + ".hmac"))
        {
            string valueHmac = sr.ReadLine();
            if (valueHmac != hash) {
                return nameFiles[i];
            }
        }

    }
    return "Deu ruim";
}

//Cria o hash hmac
static string CalcHmac(string path, int dateArchive)
{
    byte[] key = Encoding.ASCII.GetBytes(GetHashByDay(dateArchive));
    HMACSHA256 myhmacsha256 = new HMACSHA256(key);
    byte[] byteArray = File.ReadAllBytes(path);
    MemoryStream stream = new MemoryStream(byteArray);
    string result = myhmacsha256.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
    return result;
}

//Pega o hash de determinado dia
static string GetHashByDay(int dateArchive)
{
    List<string> hash = GetHash();
    string[] hashDay = hash.ToArray();

    return hashDay[2 - 1];
}

//Lista com os hash diarios
static List<string> GetHash()
{
    List<string> hash = new List<string>();
    string path = "/home/luisfernando/Desktop/Criptografia/desafio_04/stringHash.txt";
    try
    {
        using (StreamReader sr = File.OpenText(path))
        {
            while (!sr.EndOfStream)
            {
                
                string hashDay = sr.ReadLine();
                hash.Add(hashDay);
            }
        }
    }
    catch (IOException e)
    {
        System.Console.WriteLine(e.Message);
    }

    return hash;
}

//Todos os nome dos arquivos
static string[] ReadNameFiles(string pathFile)
{
    DirectoryInfo diretorio = new DirectoryInfo(pathFile);
    List<string> nameArchive = new List<string>();
    try
    {
        FileInfo[] Arquivos = diretorio.GetFiles("*.rtf");
        FileInfo[] Arquivos2 = diretorio.GetFiles("*.doc");

        Arquivos = Arquivos.Concat(Arquivos2).ToArray();
        foreach (FileInfo fileinfo in Arquivos)
        {
            nameArchive.Add(fileinfo.Name);
        }

    }
    catch (Exception e)
    {
        System.Console.WriteLine(e.Message);
    }

    return nameArchive.ToArray();
}

