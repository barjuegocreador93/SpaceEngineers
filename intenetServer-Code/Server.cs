//                 
//Server nodo=new Server( string TimerBlocksGrups,string MainDispaly,bool SaveUsers=false, string IP="0-",                  
//bool static_=true,string DatabaseIPs="DBplaces",string DatabaseUsers="DBuser",  string DatabaseRam="DBram",              
//string DBStaticToDinamic="DBStaticToDinamic" )                 
        

        
Server nodo=new Server("key","Display nodo 0",true,"0-");      
           
         
         
         
void Main()         
{         
    nodo.RemoteControl(); 
    ServerMain(ref nodo);         
}           
        
bool IsConnected(string antenna)        
{        
    return _<IMyLaserAntenna>(antenna).DetailedInfo.Split('\n')[2].Contains("Connected to")||!_<IMyLaserAntenna>(antenna).DetailedInfo.Split('\n')[2].Contains("Idle");        
}        
        
        
        
IMyTerminalBlock capturar_objeto(string _obj)        
{        
    return GridTerminalSystem.GetBlockWithName(_obj);        
}        
        
        
        
void cambiarAccionObjeto(string _objeto, string _accion)        
{        
    IMyTerminalBlock objeto = capturar_objeto(_objeto);        
    ITerminalAction accion = objeto.GetActionWithName(_accion);        
    accion.Apply(objeto);        
}        
        
bool Filter_Method_Laser_Antenna(IMyTerminalBlock block)        
{        
    IMyLaserAntenna la = block as IMyLaserAntenna;        
    return la != null;        
}        
bool Filter_Method_Timer_Block(IMyTerminalBlock block)        
{        
    IMyTimerBlock la = block as IMyTimerBlock;        
    return la != null;        
}        
string _s(string name, Func<IMyTerminalBlock, bool> collect = null)        
{        
    List<IMyTerminalBlock> list = new List<IMyTerminalBlock>();        
    GridTerminalSystem.SearchBlocksOfName(name, list, collect);        
    if (list.Count != 0)        
       {       
           for(int i=0;i<list.Count;i++)       
           {       
             if (list[i].CustomName.StartsWith(name))       
                return list[i].CustomName;       
           }       
       }       
    return "";        
}        
        
T _<T>(string name, string action = "") where T : class        
{        
    if (action.Length != 0)        
    {        
        string[] actions = action.Split(',');        
        foreach (string act in actions)        
            cambiarAccionObjeto(name, act);        
    }        
    return capturar_objeto(name) as T;        
}        
        
        
        
void __<T>(string name, string action) where T : class        
{        
    string[] names = name.Split(',');        
    foreach (string n in names)        
    {        
        for (int i = 1; exist(n + i.ToString()); i++)        
        {        
            _<T>(n + i.ToString(), action);        
        }        
    }        
}        
        
void __<T>(string name, ref List<string> blocks) where T : class        
{        
    string[] names = name.Split(',');        
    foreach (string n in names)        
    {        
        for (int i = 1; exist(n + i.ToString()); i++)        
        {        
            blocks.Add(n + i.ToString());        
        }        
    }        
}        
        
bool exist(string nombre_objeto)        
{        
    if (capturar_objeto(nombre_objeto) == null)        
        return false;        
    return true;        
}        
        
void mostrar(string text, string display, bool pub = true, bool show = true)        
{   
    _<IMyTextPanel>(display).ShowPrivateTextOnScreen();       
    if (pub) _<IMyTextPanel>(display).WritePublicText(text);        
    else _<IMyTextPanel>(display).WritePrivateText(text);        
    if (show) _<IMyTextPanel>(display).ShowPublicTextOnScreen();        
}        
        
        
int teclado(string keyup, string keydown, string keycheck)        
{        
    if (_<IMyTimerBlock>(keyup).IsCountingDown)        
    { cambiarAccionObjeto(keyup, "Stop"); return 1; }        
    if (_<IMyTimerBlock>(keydown).IsCountingDown)        
    { cambiarAccionObjeto(keydown, "Stop"); return 2; }        
    if (_<IMyTimerBlock>(keycheck).IsCountingDown)        
    { cambiarAccionObjeto(keycheck, "Stop"); return 3; }        
    return 0;        
}        
        
class database        
{        
    public List<string> filas;        
    public List<string> columnas;        
        
    //@param columna="type1,type2,typeN"                     
    //@param db="fila1,fila2,filaN"                     
    //string filaN="colum1|column2|columN"                     
    public database(string db = "null", string columna = "null")        
    {        
        filas = new List<string>();        
        columnas = new List<string>();        
        if (columna != "null" && columna.Length != 0)        
        {        
            string[] aux1 = columna.Split(',');        
            foreach (string item in aux1)        
            {        
                columnas.Add(item);        
            }        
        }        
        if (db != "null" && db.Length != 0)        
        {        
            string[] aux2 = db.Split('\n');        
            foreach (string item in aux2)        
            {        
                filas.Add(item);        
            }        
        }        
    }        
    //@param fila="colum1|column2|columN"                     
    public void Addfila(string fila)        
    {        
        filas.Add(fila);        
    }        
    public void Setfila(int Indexfila, string fila)        
    {        
        if (Indexfila >= 0 && Indexfila < filas.Count)        
            filas[Indexfila] = fila;        
    }        
    public void SetColumn(int indexfila, int indexColum, string data)        
    {        
        if (indexfila >= 0 && indexfila < filas.Count)        
        {        
            string[] fila = filas[indexfila].Split('|');        
            if (indexColum >= 0 && indexColum < fila.Length)        
            {        
                fila[indexColum] = data;        
                filas[indexfila] = "";        
                foreach (string d in fila)        
                {        
                    filas[indexfila] += d;        
                    if (d != fila[fila.Length - 1]) filas[indexfila] += "|";        
                }        
            }        
        }        
    }        
    public int GetDataInt(int Indexfila, int IndexCol)        
    {        
        
        int aux = Int32.Parse(GetDataString(Indexfila, IndexCol));        
        return aux;        
    }        
    public float GetDataFloat(int Indexfila, int IndexCol)        
    {        
        float aux;        
        aux = float.Parse(GetDataString(Indexfila, IndexCol));        
        return aux;        
    }        
    public string GetDataString(int Indexfila, int IndexCol)        
    {        
        string aux = "";        
        if (Indexfila >= 0 && Indexfila < filas.Count)        
        {        
            string[] auxColum = filas[Indexfila].Split('|');        
            int cont = 0;        
            foreach (string item in auxColum)        
            {        
                if (cont == IndexCol)        
                    return item;        
                cont++;        
            }        
        
        
        }        
        return aux;        
    }        
        
    public string Save()        
    {        
        string text = "";        
        for (int i = 0; i < filas.Count; i++)        
        {        
            text += filas[i];        
            if (i < filas.Count - 1) text += "\n";        
        }        
        return text;        
    }        
    public bool existEnFila(int fila, string var)        
    {        
        if (fila >= 0 && fila < filas.Count)        
        {        
            string[] Fila = filas[fila].Split('|');        
            foreach (string d in Fila)        
            {        
                if (d == var) return true;        
            }        
        
        }        
        return false;        
    }        
    public bool existEnColumnas(int columna, string var)        
    {        
        for (int i = 0; i < filas.Count; i++)        
        {        
            if (GetDataString(i, columna) == var)        
                return true;        
        }        
        return false;        
    }        
    public string[] FilaForColumna(int columna, string var)        
    {        
        for (int i = 0; i < filas.Count; i++)        
        {        
            if (GetDataString(i, columna) == var)        
                return filas[i].Split('|');        
        }        
        string[] nu = { "null" };        
        return nu;        
    }        
    public int IndexFilaOfColumna(int columna, string var)        
    {        
        for (int i = 0; i < filas.Count; i++)        
        {        
            if (GetDataString(i, columna) == var)        
                return i;        
        }        
        return -1;        
    }        
    public void AddUniquedFila(int indexColumn, string Columndata, string OtherColumns)        
    {        
        int i = IndexFilaOfColumna(0, Columndata);        
        if (i > -1)        
        {        
            filas[i] = Columndata + "|" + OtherColumns;        
        }        
        else        
        {        
            filas.Add(Columndata + "|" + OtherColumns);        
        }        
    }        
        
        
}        
        
class menu        
{        
    private int cursorPosition;        
    private int tecla;        
    private string title;        
    private string addtext;        
    private List<string> options;        
    private List<int> sig;        
    private IMyTextPanel display;        
        
    public menu(string titulo, string aditionaltext)        
    {        
        title = titulo;        
        addtext = aditionaltext;        
        options = new List<string>();        
        sig = new List<int>();        
        cursorPosition = 1;        
    }        
    public void addOption(string name, int menu_sig)        
    {        
        options.Add(name);        
        sig.Add(menu_sig);        
    }        
    public int teclado(int teclaon)        
    {        
        if (teclaon == 1)        
        {        
            tecla = 1;        
            if (cursorPosition > 1)        
                cursorPosition--;        
            return 1;        
        }        
        else        
        if (teclaon == 2)        
        {        
            tecla = 2;        
            if (cursorPosition < options.Count)        
                cursorPosition++;        
            return 2;        
        }        
        else        
        if (teclaon == 3)        
        {        
            return tecla = 3;        
        
        }        
        else        
        if (teclaon == 4)        
        {        
            return tecla = 4;        
        }        
        return 0;        
    }        
    public string mostrar()        
    {        
        string text = title + "\n";        
        text += addtext + "\n";        
        for (int i = 0; i < options.Count; i++)        
        {        
            if (cursorPosition - 1 == i)        
                text += " > " + (i + 1).ToString() + ". " + options[i] + "\n";        
            else text += "   " + (i + 1).ToString() + ". " + options[i] + "\n";        
        }        
        return text;        
        
    }        
    public int GetTecla()        
    {        
        return tecla;        
    }        
    public int GetCusrorPosition()        
    {        
        return cursorPosition;        
    }        
    public int GetSig(int cursor_position)        
    {        
        return sig[cursor_position - 1];        
    }        
        
}        
        
class interfaz        
{        
    public List<menu> ifcs;        
    public int ifact;        
    private bool ready = true;        
    public interfaz()        
    {        
        ifcs = new List<menu>();        
        ifact = 0;        
    }        
    public void addMenu(string titulo, string aditionaltext)        
    {        
        menu aux = new menu(titulo, aditionaltext);        
        ifcs.Add(aux);        
    }        
    public void addOptionIn(int index, string name, int sig)        
    {        
        ifcs[index].addOption(name, sig);        
    }        
    public List<int> run(int teclaon)//return 0=tecla 1=cursorposition 2=ifact                   
    {        
        List<int> aux = new List<int>();        
        aux.Add(ifcs[ifact].teclado(teclaon));        
        aux.Add(ifcs[ifact].GetCusrorPosition());        
        aux.Add(ifact);        
        if (aux[0] == 3)        
        {        
            ifact = ifcs[ifact].GetSig(aux[1]);        
        }        
        
        return aux;        
    }        
    public int GetIfact()        
    {        
        return ifact;        
    }        
    public void SetIfact(int index)        
    {        
        if (index >= 0 && index < ifcs.Count)        
            ifact = index;        
    }        
    public void makeUsingDataBase(string db)        
    {        
        if (ready)        
        {        
            database dbid = new database(db);        
            for (int i = 0; i < dbid.filas.Count; i++)        
            {        
                string[] colum = dbid.FilaForColumna(0, dbid.GetDataString(i, 0));        
                string[] text = colum[1].Split('#');        
                colum[1] = "";        
                foreach (string n in text)        
                    colum[1] += n + "\n";        
                addMenu(colum[0], colum[1]);        
                foreach (string options in colum)        
                {        
                    if (options != colum[0] && options != colum[1])        
                    {        
                        int sig;        
                        string[] option = options.Split(';');        
                        if (dbid.existEnColumnas(0, option[1]))        
                        {        
                            sig = dbid.IndexFilaOfColumna(0, option[1]);        
                        }        
                        else        
                        {        
                            sig = int.Parse(option[1]);        
                        }        
        
                        addOptionIn(i, option[0], sig);        
                    }        
                }        
            }        
            ready = false;        
        }        
    }        
    public void Clear(int ifacts)        
    {        
        ready = true;        
        ifcs.Clear();        
        ifact = ifacts;        
    }        
}        
        
class Server        
{        
    public List<string> Components = new List<string>();        
    public interfaz id = new interfaz();        
    public string IP;        
    public bool Static;        
    public List<string> msg = new List<string>();        
    public List<string> senval = new List<string>();        
    public database ifaz = new database(        
        "Main Menu|Use this panel to create a new #IP: |Create Point;1|" +        
        "Multi ip Point;Add->Multi ip Point|AutoIP;AutoIP\n" +        
        "Add->ip|Are you sure to create a new point?|Yes;0|No;0\n" +        
        "AutoIP|Select|active/desabled;AutoIP|Back;0\n" +        
        "Add->Multi ip Point||s;0"        
    );        
    public bool autoIp = true;        
    public List<string> mp = new List<string> { "", "" };        
    public bool ready = true;        
    public bool SaveUsersHere = false;        
    public Server(string timerblocks, string textpanel, bool SaveUser = false, string ip = "0-", bool static_ = true,        
    string DBplaces = "DBplaces", string DBusers = "DBuser",        
    string DBram = "DBram", string DBStaticToDinamic = "DBStaticToDinamic")        
    {        
        Components.Add(timerblocks);//0 keys and time                 
        Components.Add(textpanel);//1 visual                 
        Components.Add(DBplaces);//2 save place : antennas                 
        Components.Add(DBusers);//3 save user                 
        Components.Add(DBram);//4 processes              
        Components.Add(DBStaticToDinamic);//5 static to dinamics ips                 
        SaveUsersHere = SaveUser;        
        IP = ip;        
        Static = static_;        
    }  
      
    public bool remotecontrol=false;  
    public bool assis=true; 
    public void RemoteControl(bool rc=true,string DBrc="DBrc")  
    {  
        if(assis){ 
        Components.Add(DBrc);  
        remotecontrol=rc; assis=false;} 
    }        
    public void SupliteMSGAntenna(string dbplace)        
    {        
        if (ready)        
        {        
            database p = new database(dbplace);        
            for (int i = 0; i < p.filas.Count; i++)        
                msg.Add("");        
            ready = false;        
        }        
    }        
        
}        
void AutoIP(ref Server nodo)        
{        
    string la = _s("Laser Antenna", Filter_Method_Laser_Antenna);        
    string tb = _s("Timer Block", Filter_Method_Timer_Block);        
    if (la != "" && tb != "")        
    {        
        if (_<IMyTerminalBlock>(la).HasLocalPlayerAccess() && _<IMyTerminalBlock>(tb).HasLocalPlayerAccess())        
        {        
            database place = new database(_<IMyTextPanel>(nodo.Components[2]).GetPublicText());        
            if (place.filas.Count != 0)        
            {        
                string[] ip = place.GetDataString(place.filas.Count - 1, 0).Split('-');        
                int i = int.Parse(ip[ip.Length - 2]) + 1;        
                string newIp = "";        
                for (int x = 0; x < ip.Length - 2; x++)        
                    newIp += ip[x] + "-";        
                newIp += i + "-";        
                _<IMyTerminalBlock>(la).SetCustomName(newIp);        
                _<IMyTerminalBlock>(tb).SetCustomName(newIp.Replace('-', '.'));        
                place.filas.Add(newIp);        
                _<IMyTextPanel>(nodo.Components[2]).WritePublicText(place.Save());        
                nodo.msg.Add("");        
            }        
            else        
            {        
                _<IMyTerminalBlock>(la).SetCustomName(nodo.IP);        
                _<IMyTerminalBlock>(tb).SetCustomName(nodo.IP.Replace('-', '.'));        
                place.filas.Add(nodo.IP);        
                _<IMyTextPanel>(nodo.Components[2]).WritePublicText(place.Save());        
                nodo.msg.Add("");        
            }        
        }        
    }        
}        
void ServerMain(ref Server nodo)        
{        
    if (nodo.autoIp) AutoIP(ref nodo);        
    nodo.SupliteMSGAntenna(_<IMyTextPanel>(nodo.Components[2]).GetPublicText());        
    nodo.id.makeUsingDataBase(nodo.ifaz.Save());        
        
    List<int> moves = nodo.id.run(teclado(nodo.Components[0] + " 1", nodo.Components[0] + " 2", nodo.Components[0]        
    + " 3"));        
    if (moves[0] == 3)        
    {        
        database place = new database(_<IMyTextPanel>(nodo.Components[2]).GetPublicText());        
        if (moves[2] == 1 && moves[1] == 1)        
        {        
        
            if (place.filas.Count != 0)        
            {        
                string[] ip = place.GetDataString(place.filas.Count - 1, 0).Split('-');        
                int i = int.Parse(ip[ip.Length - 2]) + 1;        
                string newIp = "";        
                for (int x = 0; x < ip.Length - 2; x++)        
                    newIp += ip[x] + "-";        
                newIp += i + "-";        
                place.filas.Add(newIp);        
                _<IMyTextPanel>(nodo.Components[2]).WritePublicText(place.Save());        
                nodo.msg.Add("");        
            }        
            else        
            {        
                place.filas.Add(nodo.IP);        
                _<IMyTextPanel>(nodo.Components[2]).WritePublicText(place.Save());        
                nodo.msg.Add("");        
            }        
        }        
        if (moves[1] == 2 && moves[2] == 0)        
        {        
            nodo.id.Clear(2);        
        
        
            string ips = "";        
            for (int i = 0; i < place.filas.Count; i++)        
            {        
                ips += place.GetDataString(i, 0) + ";2|";        
            }        
            ips += "Back;0";        
            nodo.ifaz.AddUniquedFila(0, "Add->Multi ip Point", "Select one of this:|" + ips);        
            nodo.id.makeUsingDataBase(nodo.ifaz.Save());        
            nodo.id.ifact = nodo.ifaz.IndexFilaOfColumna(0, "Add->Multi ip Point");        
        }        
        if (moves[1] == 3 && moves[2] == 0)        
        {        
            nodo.id.Clear(3);        
            nodo.ifaz.AddUniquedFila(0, "AutoIP", "status: " + nodo.autoIp.ToString() + "|Enable/Disabled;AutoIP|Back;0");        
            nodo.id.makeUsingDataBase(nodo.ifaz.Save());        
            nodo.id.ifact = nodo.ifaz.IndexFilaOfColumna(0, "AutoIP");        
        
        }        
        if (moves[1] == 1 && moves[2] == nodo.ifaz.IndexFilaOfColumna(0, "AutoIP"))        
        {        
            nodo.id.Clear(3);        
            if (nodo.autoIp) nodo.autoIp = false;        
            else nodo.autoIp = true;        
            nodo.ifaz.AddUniquedFila(0, "AutoIP", "status: " + nodo.autoIp.ToString() + "|Enable/Disabled;AutoIP|Bakc;0");        
            nodo.id.makeUsingDataBase(nodo.ifaz.Save());        
            nodo.id.ifact = nodo.ifaz.IndexFilaOfColumna(0, "AutoIP");        
        }        
        for (int i = 0; i < place.filas.Count; i++)        
        {        
            if (moves[1] == i + 1 && moves[2] == nodo.ifaz.IndexFilaOfColumna(0, "Add->Multi ip Point"))        
            {        
                nodo.id.Clear(0);        
                string ips = "";        
                for (int j = 0; j < place.filas.Count; j++)        
                {        
                    for (int x = 0; x < place.FilaForColumna(0, place.GetDataString(i, 0)).Length; x++)        
                    {        
                        if (place.FilaForColumna(0, place.GetDataString(i, 0))[x] == place.GetDataString(j, 0))        
                            j++;        
                    }        
                    ips += place.GetDataString(j, 0) + ";0|";        
                }        
                ips += "Back;" + nodo.ifaz.IndexFilaOfColumna(0, "Add->Multi ip Point").ToString();        
                nodo.ifaz.AddUniquedFila(0, "Add->Multi ip Point->to", "Select one of this:|" + ips);        
                nodo.id.makeUsingDataBase(nodo.ifaz.Save());        
                nodo.id.ifact = nodo.ifaz.IndexFilaOfColumna(0, "Add->Multi ip Point->to");        
                nodo.mp[0] = nodo.ifaz.GetDataString(nodo.ifaz.IndexFilaOfColumna(0, "Add->Multi ip Point"), 2 + i)        
                .Split(';')[0];        
                i = place.filas.Count;        
            }        
        }        
        for (int i = 0; i < place.filas.Count - 1; i++)        
        {        
            if (moves[1] == i + 1 && moves[2] == nodo.ifaz.IndexFilaOfColumna(0, "Add->Multi ip Point->to"))        
            {        
                nodo.mp[1] = nodo.ifaz.GetDataString(nodo.ifaz.IndexFilaOfColumna(0, "Add->Multi ip Point->to"), 2 + i)        
                .Split(';')[0];        
                int j = place.IndexFilaOfColumna(0, nodo.mp[0]);        
                if (!place.existEnFila(j, nodo.mp[1])) place.filas[j] += "|" + nodo.mp[1];        
                j = place.IndexFilaOfColumna(0, nodo.mp[1]);        
                if (!place.existEnFila(j, nodo.mp[0])) place.filas[j] += "|" + nodo.mp[0];        
                _<IMyTextPanel>(nodo.Components[2]).WritePublicText(place.Save());        
            }        
        }        
    }        
    //Save Message in a RAM                  
    database p = new database(_<IMyTextPanel>(nodo.Components[2]).GetPublicText());        
    database m = new database(_<IMyTextPanel>(nodo.Components[4]).GetPublicText());          
            
    for (int i = 0; i < p.filas.Count; i++)        
    {        
        string antenna = _s(p.GetDataString(i, 0), Filter_Method_Laser_Antenna);        
        if (antenna != "" && exist(p.GetDataString(i, 0).Replace('-', '.')))        
        {        
            if (IsConnected(antenna))        
            {        
                string antInfo = _<IMyTerminalBlock>(antenna).DetailedInfo.Replace('\n', 'x');        
                database at2 = new database(antInfo);        
        
                string[] msg = at2.filas[0].Split('|');        
                string[] infoEr = msg[0].Split(' ');        
                string[] antenna2 = infoEr[infoEr.Length - 1].Split('-');        
                if (antenna2.Length > 1)        
                {        
                    string message = antenna2[antenna2.Length - 1];        
                    for (int j = 1; j < msg.Length; j++) message += "|" + msg[j];        
                    if (nodo.msg[i] != message) { m.filas.Add(message); nodo.msg[i] = message; }        
                }        
            }        
        }        
        else        
        {        
            if (exist(p.GetDataString(i, 0).Replace('-', '.')))        
            {        
                if (_<IMyTerminalBlock>(p.GetDataString(i, 0).Replace('-', '.')).HasLocalPlayerAccess())        
                    _<IMyTerminalBlock>(p.GetDataString(i, 0).Replace('-', '.')).SetCustomName("Timer Block -");        
            }        
            if (antenna != "")        
            {        
                if (_<IMyTerminalBlock>(antenna).HasLocalPlayerAccess())        
                    _<IMyTerminalBlock>(antenna).SetCustomName("Laser Antenna -");        
            }        
            p.filas.RemoveAt(i);        
            nodo.msg.RemoveAt(i);        
            i--;        
        }        
    }        
    _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());        
    _<IMyTextPanel>(nodo.Components[2]).WritePublicText(p.Save());        
        
    for (int i = 0; i < m.filas.Count;)        
    {        
            
        string typeRq = m.GetDataString(i, 0).Split('#')[0];        
        if (typeRq == "clear")        
        {    
                  
            m.filas.RemoveAt(i);        
            _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());        
        }        
        else {        
            string[] ipd = m.GetDataString(i, 0).Split('#');        
            string url = "";        
            if (ipd.Length > 1)        
            {        
                url = ipd[1].Replace('.', '-');        
            }     
                
            switch (ipd[0])        
            {        
                case "disp":        
                    i++;        
                    break;        
                case "regist":        
                    if (nodo.SaveUsersHere && Server_PortsEquals(url, ref p)) Server_Regist_User(ref m, ref i, ref nodo);        
                    else Server_SendMSG(url, ref p, i, ref m);        
                    break;        
                case "sesion": Server_Sesion(url, ref p, ref i, ref m); break;        
                case "msg":        
                    if (!Server_SendMSG(url, ref p, i, ref m))        
                    {        
                        //dips-por terminar-670             
        
        
                        if (p.existEnColumnas(0, m.GetDataString(i, 2).Replace('.', '-')))        
                        {        
        
        
                            string[] msg = m.GetDataString(i, 3).Split(',');        
                            if (msg.Length == 2)        
                            {        
                                switch (msg[0])        
                                {        
                                    case "Quest": //msg#ip|device,ip|ip|Quest,type|serial                 
                                        if (msg[1] == "saveUser")        
                                        {        
                                            string ans;        
                                            if (nodo.SaveUsersHere) ans = "true";        
                                            else ans = "false";        
                                            if (m.GetDataString(i, 1).Split(',').Length == 2)        
                                            {        
                                                m.filas.Add("msg#" + m.GetDataString(i, 1).Split(',')[1] + "|" +        
                                                p.GetDataString(0, 0).Replace('-', '.') + "|" + m.GetDataString(i, 1).Split(',')[0] + "|saveUser,"        
                                                + ans + "|" + m.GetDataString(i, 4));        
                                                m.filas.RemoveAt(i);        
                                                _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());        
                                            }        
                                            else i++;        
                                        }        
                                        else i++;        
                                        break;        
                                    default: i++; break;        
                                }        
                                i++;        
                            }        
                            else i++;        
        
                        }        
                        else        
                            i++;        
        
        
                    }        
                    break;        
                default: i++; break;        
        
            }        
        }        
        
    }        
    Server_Dips(ref nodo, ref m, ref p);        
    Server_clear_chanels(ref nodo, ref p);        
    Server_StaticToDinamicRed(ref nodo, ref p);        
    mostrar(nodo.id.ifcs[nodo.id.GetIfact()].mostrar(), nodo.Components[1]);        
    _<IMyTextPanel>(nodo.Components[1], "OnOff");        
    _<IMyTextPanel>(nodo.Components[1], "OnOff");        
    _<IMyTimerBlock>(nodo.Components[0] + " 4", "Start");        
        
}        
        
        
void Server_Regist_User(ref database m, ref int i, ref Server nodo)        
{        
    string IdUser = m.GetDataString(i, 1);        
    string name = m.GetDataString(i, 2);        
    string password = m.GetDataString(i, 3);        
    string IPPersonalDatabaseInbox = m.GetDataString(i, 4);        
    string NamePersonalDatabaseInbox = m.GetDataString(i, 5);        
    string status = m.GetDataString(i, 6);        
    string IPmade = m.GetDataString(i, 7);        
    string Device = m.GetDataString(i, 8);        
    string serial = m.GetDataString(i, 9);        
        
        
    if (status == "quest")        
    {        
        database user = new database(_<IMyTextPanel>(nodo.Components[3]).GetPublicText());        
        if (!user.existEnColumnas(0, IdUser))        
        {        
            status = "logOut";        
            user.filas.Add(IdUser + "|" + name + "|" + password + "|" + IPPersonalDatabaseInbox + "|" + NamePersonalDatabaseInbox        
            + "|" + status + "|" + IPmade + "|" + Device);        
        
            _<IMyTextPanel>(nodo.Components[3]).WritePublicText(user.Save());        
            m.filas.RemoveAt(i);        
            m.filas.Add("msg#" + IPmade + "|" + nodo.IP.Replace('-', '.') + "|" +        
                    Device + "|regist,true|" + serial);        
            _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());        
        }        
        else        
        {        
            m.filas.RemoveAt(i);        
            m.filas.Add("msg#" + IPmade + "|" + nodo.IP.Replace('-', '.') + "|" +        
                Device + "|regist,false|" + serial);        
            _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());        
        }        
        
    }        
    else        
    {        
        m.filas.RemoveAt(i);        
        _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());        
    }        
        
        
}        
void Server_Sesion(string url, ref database p, ref int i, ref database m)        
{        
    if (!Server_SendMSG(url, ref p, i, ref m))        
    {        
        if (nodo.SaveUsersHere && Server_PortsEquals(url, ref p))        
        {        
        
        
            database user = new database(_<IMyTextPanel>(nodo.Components[3]).GetPublicText());        
            if (user.existEnColumnas(0, m.GetDataString(i, 2)))        
            {        
                string status = m.GetDataString(i, 1);        
                switch (status)        
                {        
                    case "Sign": //Sign | user | password | device | ipDevice | |serial                  
                        if (user.FilaForColumna(0, m.GetDataString(i, 2))[2] == m.GetDataString(i, 3))        
                        {        
                            int index = user.IndexFilaOfColumna(0, m.GetDataString(i, 2));        
                            user.SetColumn(index, 5, status);        
                            user.SetColumn(index, 6, m.GetDataString(i, 5));        
                            user.SetColumn(index, 7, m.GetDataString(i, 4));        
                            _<IMyTextPanel>(nodo.Components[3]).WritePublicText(user.Save());        
                            m.filas.Add("msg#" + m.GetDataString(i, 5).Replace('-', '.') + "|" +        
                            p.GetDataString(0, 0).Replace('-', '.') + "|" + m.GetDataString(i, 4) + "|online,true|" + m.GetDataString(i, 6));        
                            _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());        
                        }        
                        else        
                        {        
                            m.filas.Add("msg#" + m.GetDataString(i, 5).Replace('-', '.') + "|" +        
                            p.GetDataString(0, 0).Replace('-', '.') + "|" + m.GetDataString(i, 4) + "|online,false|" + m.GetDataString(i, 6));        
                            _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());        
                        }        
                        break;        
                    case "logOut": //logOut | user | password | device | ipDevice | serial                   
                        if (user.FilaForColumna(0, m.GetDataString(i, 2))[2] == m.GetDataString(i, 3))        
                        {        
                            int index = user.IndexFilaOfColumna(0, m.GetDataString(i, 2));        
                            if (user.GetDataString(index, 6).Replace('-', '.') == m.GetDataString(i, 5).Replace('-', '.') &&        
                                    user.GetDataString(index, 7) == m.GetDataString(i, 4))        
                            {        
                                user.SetColumn(index, 5, status);        
                                _<IMyTextPanel>(nodo.Components[3]).WritePublicText(user.Save());        
                                m.filas.Add("msg#" + m.GetDataString(i, 5).Replace('-', '.') + "|" +        
                                p.GetDataString(0, 0).Replace('-', '.') + "|" + m.GetDataString(i, 4) + "|online,false|" + m.GetDataString(i, 6));        
                                _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());        
                            }        
                        }        
                        break;        
                }        
            }        
        
        }        
        m.filas.RemoveAt(i);        
        _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());        
    }        
    else i++;        
}        
        
        
bool Server_SendMSG(string url, ref database p, int i, ref database m)        
{        
    if(url.Length>0){    
    if (!Server_PortsEquals(url, ref p))        
    {        
        string ipUrl = Server_WayMsg(url, ref p);        
        if (ipUrl != "null")        
        {        
            string antenna;        
            if (_<IMyTimerBlock>(ipUrl.Replace('-', '.')).IsCountingDown ||        
            !IsConnected(_s(ipUrl, Filter_Method_Laser_Antenna)))        
            {        
                string same;        
                bool ok = true;        
                for (int x = 1; x < p.FilaForColumna(0, ipUrl).Length && ok; x++)        
                {        
                    same = p.GetDataString(p.IndexFilaOfColumna(0, ipUrl), x);        
                    string sameA = _s(same, Filter_Method_Laser_Antenna);        
                    if (!_<IMyTimerBlock>(same.Replace('-', '.')).IsCountingDown &&        
                    IsConnected(_s(same, Filter_Method_Laser_Antenna)))        
                    { ipUrl = same; ok = false; }        
                }        
                if (ok) return false;        
        
            }       
            if(IsConnected(_s(ipUrl,Filter_Method_Laser_Antenna))){       
            antenna = _s(ipUrl, Filter_Method_Laser_Antenna);        
            _<IMyTerminalBlock>(antenna).SetCustomName(ipUrl + m.filas[i]);        
            antenna = _s(ipUrl, Filter_Method_Laser_Antenna);        
            _<IMyLaserAntenna>(antenna, "OnOff_Off");        
            _<IMyLaserAntenna>(antenna, "OnOff_On");        
            _<IMyTimerBlock>(ipUrl.Replace('-', '.'), "Start");       
             m.filas.RemoveAt(i);        
            _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());         
            }       
        
        }        
              
        return true;        
    }    
    }else     
    {    
         m.filas.RemoveAt(i);         
         _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());    
         return true;    
    }        
    return false;        
        
}        
        
bool Server_PortsEquals(string IP, ref database p)        
{        
    for (int i = 0; i < p.filas.Count; i++)        
        if (p.GetDataString(i, 0) == IP) return true;        
    return false;        
}        
        
void Server_clear_chanels(ref Server sv, ref database p)        
{        
    for (int i = 0; i < p.filas.Count; i++)        
    {        
        var ornam = p.GetDataString(i, 0);        
        var timer = ornam.Replace('-', '.');        
        var ant = _s(ornam, Filter_Method_Laser_Antenna);        
        if (p.filas.Count != sv.senval.Count)        
            sv.senval.Add("-");        
        
        if (!_<IMyTimerBlock>(timer).IsCountingDown)        
        {        
            if (sv.senval[i] == "")        
            {        
                _<IMyTerminalBlock>(ant).SetCustomName(ornam + "clear");        
                _<IMyLaserAntenna>(ornam + "clear", "OnOff_Off");        
                _<IMyLaserAntenna>(ornam + "clear", "OnOff_On");        
                sv.senval[i] = "-";        
            }        
        
        
        }        
        else sv.senval[i] = "";        
        
    }       
    if(sv.senval.Count>p.filas.Count)       
        for(int i=p.filas.Count-1;i<sv.senval.Count;i++)       
            sv.senval.RemoveAt(i);        
}        
        
string Server_WayMsg(string url, ref database p)        
{        
    for (int i = 0; i < p.filas.Count; i++)        
    {       
                
        string w=p.GetDataString(i,0);       
        w.Replace('.','-');       
        string[] dw=w.Split('-');       
        int cifras=dw[dw.Length-2].Length;       
        
        string result;        
        if (url.Length >= p.GetDataString(i, 0).Length)        
        {        
            string surl = url.Remove(p.GetDataString(i, 0).Length - (1 + cifras));        
            string sip = p.GetDataString(i, 0).Remove(p.GetDataString(i, 0).Length - (1 + cifras));        
            if (surl != sip) result = p.GetDataString(0, 0);        
            else result = url.Remove(p.GetDataString(i, 0).Length);        
        }        
        else result = p.GetDataString(0, 0);        
        
        if (Server_PortsEquals(result, ref p)) return result;        
    }        
    return "null";        
}        
        
void Server_StaticToDinamicRed(ref Server n, ref database p)        
{        
    if (n.Static)        
    {        
        database std = new database(_<IMyTextPanel>(n.Components[5]).GetPublicText());        
        for (int i = 0; i < p.filas.Count; i++)        
        {        
            string ip = p.GetDataString(i, 0);        
            string port = _s(ip, Filter_Method_Laser_Antenna);        
            if (_<IMyTerminalBlock>(port).HasLocalPlayerAccess())        
            {        
                string[] k = std.FilaForColumna(0, ip);        
                if (!_<IMyLaserAntenna>(port).IsPermanent)        
                {        
                    string gpsP = _<IMyCubeBlock>(port).Position.ToString();        
                    if (k.Length == 4)        
                    {        
                        if (p.existEnColumnas(0, ip) && k[1] != gpsP)        
                        {        
        
                            string data = gpsP + "|new|here";        
                            std.AddUniquedFila(0, ip, data);        
        
                            _<IMyTextPanel>(n.Components[5]).WritePublicTitle("new");        
                            mostrar(std.Save(), n.Components[5]);        
                        }        
                    }        
                    else        
                    {        
                        string data = gpsP + "|new|here";        
                        std.AddUniquedFila(0, ip, data);        
        
                        _<IMyTextPanel>(n.Components[5]).WritePublicTitle("new");        
                        mostrar(std.Save(), n.Components[5]);        
                    }        
                }        
        
        
            }        
        }        
        for (int x = 0; x < std.filas.Count; x++)        
        {        
            if (!p.existEnColumnas(0, std.GetDataString(x, 0)) && std.GetDataString(x, 3) == "here")        
            {        
                string[] data = std.FilaForColumna(0, std.GetDataString(x, 0));        
                data[2] = "del";        
                std.AddUniquedFila(0, data[0], data[1] + "|" + data[2] + "|" + data[3]);        
                _<IMyTextPanel>(n.Components[5]).WritePublicTitle("new");        
                mostrar(std.Save(), n.Components[5]);        
            }        
            if (std.GetDataString(x, 3) == "here")        
            {        
                string port_ = _s(std.GetDataString(x, 0), Filter_Method_Laser_Antenna);        
                if (exist(port_))        
                {        
                    if (_<IMyTerminalBlock>(port_).HasLocalPlayerAccess())        
                        if (_<IMyLaserAntenna>(port_).IsPermanent)        
                        {        
                            string[] data = std.FilaForColumna(0, std.GetDataString(x, 0));        
                            data[2] = "del";        
                            std.AddUniquedFila(0, data[0], data[1] + "|" + data[2] + "|" + data[3]);        
                            _<IMyTextPanel>(n.Components[5]).WritePublicTitle("new");        
                            mostrar(std.Save(), n.Components[5]);        
                        }        
                }        
                else        
                {        
        
                    string[] data = std.FilaForColumna(0, std.GetDataString(x, 0));        
                    data[2] = "del|here";        
                    std.AddUniquedFila(0, data[0], data[1] + "|" + data[2]);        
                    _<IMyTextPanel>(n.Components[5]).WritePublicTitle("new");        
                    mostrar(std.Save(), n.Components[5]);        
                }        
            }        
        }        
        if (_<IMyTextPanel>(n.Components[5]).GetPublicTitle() == "new")        
        {        
            string data = "";        
            string aux = "";        
            int cont = 0;        
            for (int i = 0; i < std.filas.Count; i++)        
            {        
                if (std.GetDataString(i, 2) == "new" || std.GetDataString(i, 2) == "del")        
                {        
                    cont++;        
                }        
            }        
            int z = 1;        
            List<string> pips=new  List<string>();       
            for (int i = 0; i < std.filas.Count;)        
            {        
                if (std.GetDataString(i, 2) == "new" || std.GetDataString(i, 2) == "del")        
                {        
                    data += std.filas[i];        
                    if (std.GetDataString(i, 2) == "del") std.filas.RemoveAt(i);        
                    else {        
                        string[] data_ = std.FilaForColumna(0, std.GetDataString(i, 0));        
                        data_[2] = "added";        
                        std.AddUniquedFila(0, data_[0], data_[1] + "|" + data_[2] + "|" + data_[3]);        
                        pips.Add(data_[3]);       
        
                        i++;        
                    }        
        
                    if (z < cont)        
                        data += ">";        
                    z++;        
                }        
                else i++;        
            }        
            _<IMyTextPanel>(n.Components[5]).WritePublicTitle("");        
            mostrar(std.Save(), n.Components[5]);        
            data = data.Replace('|', '<');        
            data = data.Replace('>', '|');        
        
        
            database m = new database(_<IMyTextPanel>(n.Components[4]).GetPublicText());        
            string msg;        
            string ip;        
            ip = p.GetDataString(0, 0);        
            data = "|dips|" + ip + "|" + data;    
            string[] sip=ip.Split('-');        
            msg = "msg#" + ip.Substring(0, ip.Length - 1 - sip[sip.Length-2].Length).Replace('-', '.') + data;               
              
            if(ip=="0-"&&Server_IPSPS(ref pips,ip))    
                {    
                    msg = "msg#" + ip.Replace('-', '.') + "0." + data;         
                    m.filas.Add(msg);     
                }   
            else  if(Server_IPSPS(ref pips,ip)) m.filas.Add(msg);            
                   
            for (int i = 1; i < p.filas.Count; i++)        
            {        
                ip = p.GetDataString(i, 0);                   
                            
                if (!(Server_IPisRelactional(ip, ref p)) && Server_IPisConnect(ip)&&Server_IPSPS(ref pips,ip))        
                {        
                    msg = "msg#" + ip.Replace('-', '.') + "0." + data;        
                    m.filas.Add(msg);       
                            
                }        
                        
            }        
        
            mostrar(m.Save(), n.Components[4]);        
        }        
        
        
    }        
}        
bool Server_IPisRelactional(string ip, ref database p)        
{        
    int iip = p.IndexFilaOfColumna(0, ip);        
    for (int i = 0; i < iip; i++)        
        if (p.existEnFila(i, ip)) return true;        
    return false;        
}        
bool Server_IPEqDIP(string ip, ref database std)        
{        
    for (int i = 0; i < std.filas.Count; i++)        
        if (std.existEnColumnas(0, ip)) return true;        
    return false;        
}        
bool Server_IPisConnect(string ip)        
{        
   string port=_s(ip,Filter_Method_Laser_Antenna);        
           
   if(_<IMyTerminalBlock>(port).HasLocalPlayerAccess())        
   {        
       return (IsConnected(port));        
               
   }        
   return false;        
        
}        
      
        
bool Server_IPSPS(ref List<string> pips,string ip)       
{       
    for(int i=0;i<pips.Count;i++)       
    {       
        if(ip+"0-"==pips[i] || ip==pips[i])return false;       
    }       
    return true;       
}        
        
void Server_Dips(ref Server n, ref database m, ref database p)        
{        
        
    for (int i = 0; i < m.filas.Count;)        
    {        
        string iden=m.GetDataString(i, 1) ;     
        switch(iden){     
        case "dips":        
        {        
            string[] en=m.GetDataString(i, 0).Split('#');      
                  
            if(en.Length>1){      
            if (Server_PortsEquals(en[1].Replace('.','-'),ref p))        
            {        
                string pip = m.GetDataString(i, 2);        
                string[] msg_ = m.filas[i].Split('|');        
                string data = "";        
                for (int j = 3; j < msg_.Length; j++)        
                {        
                    data += msg_[j];        
                    if (j < msg_.Length - 1) data += "\n";        
        
                }        
                data = data.Replace('<', '|');        
                database dbd = new database(data);        
                for (int j = 0; j < dbd.filas.Count; j++)        
                {        
                    dbd.SetColumn(j, 3, en[1].Replace('.','-'));        
                }        
                database std = new database(_<IMyTextPanel>(nodo.Components[5]).GetPublicText());        
                for (int j = 0; j < dbd.filas.Count; j++)        
                {        
                    std.AddUniquedFila(0, dbd.GetDataString(j, 0),        
                        dbd.GetDataString(j, 1) + "|" + dbd.GetDataString(j, 2) + "|" + dbd.GetDataString(j, 3));        
                }        
                _<IMyTextPanel>(n.Components[5]).WritePublicText(std.Save());        
                m.filas.RemoveAt(i);        
                _<IMyTextPanel>(n.Components[4]).WritePublicText(m.Save());       
                _<IMyTextPanel>(n.Components[5]).WritePublicTitle("new");        
                       
            }        
            else i++; }      
            else i++;       
                  
        
        }break;     
        case "dips.act":     
        {     
            string[] en=m.GetDataString(i, 0).Split('#');       
                   
            if(en.Length>1){       
                if (Server_PortsEquals(en[1].Replace('.','-'),ref p))         
                {        
                    List<string> pips=new List<string>{m.GetDataString(i,0).Split('#')[1].Replace('.','-')};     
                    database std = new database(_<IMyTextPanel>(nodo.Components[5]).GetPublicText());     
                    for(int j=0;j<std.filas.Count;)     
                    {     
                        if(std.GetDataString(j,3)=="here")     
                        {     
                            std.filas.RemoveAt(j);     
                        }else j++;     
                    }     
                    string ip,msg;     
                    ip =p.GetDataString(0,0);    
                    string[] sip=ip.Split('-');     
                    if(Server_IPSPS(ref pips,ip))     
                            m.filas.Add("msg#" + ip.Substring(0, ip.Length - 1 - sip[sip.Length-2].Length).Replace('-', '.') + "|dips.act");      
                    if(ip=="0-"&&Server_IPSPS(ref pips,ip))   
                        {   
                            msg = "msg#" + ip.Replace('-', '.') + "0." + "|dips.act";         
                            m.filas.Add(msg);    
                        }  else   
                            if(Server_IPSPS(ref pips,ip))      
                                    m.filas.Add("msg#" + ip.Substring(0, ip.Length - 1 - sip[sip.Length-2].Length).Replace('-', '.') + "|dips.act");       
                          
                    for(int j=1;j<p.filas.Count;j++)     
                    {                                    
                        ip =p.GetDataString(j,0);            
                        if (!(Server_IPisRelactional(ip, ref p)) && Server_IPisConnect(ip)&&Server_IPSPS(ref pips,ip))        
                        {        
                            msg = "msg#" + ip.Replace('-', '.') + "0." + "|dips.act";        
                            m.filas.Add(msg);       
                                  
                        }        
                    }                
                          
                    _<IMyTextPanel>(n.Components[5]).WritePublicText(std.Save());     
                    m.filas.RemoveAt(i);        
                    _<IMyTextPanel>(n.Components[4]).WritePublicText(m.Save());     
                }else i++;  
            }else i++;    
        break;}  
        case "ControlRemote":{  
        string[] en=m.GetDataString(i, 0).Split('#');       
                   
            if(en.Length>1){       
                if (Server_PortsEquals(en[1].Replace('.','-'),ref p))         
                {        
                   var name=m.GetDataString(i, 2);  
                   var actions=m.GetDataString(i, 3);  
                   if(name[0]=='*')  
                        __<IMyTerminalBlock>(name.Substring(1,name.Length-1),actions);  
                   else _<IMyTimerBlock>(name,actions);  
                                  
                   m.filas.RemoveAt(i);        
                   _<IMyTextPanel>(n.Components[4]).WritePublicText(m.Save());   
                      
                }else i++;  
            }else i++;   
        break; } 
            
        default: i++; break;       
       }       
    }  
    Server_RemoteControl(ref n);        
}  
  
  
void Server_RemoteControl(ref Server n)  
{  
    if(n.remotecontrol)  
    {  
        database dbrc=new database(_<IMyTextPanel>(n.Components[6]).GetPublicText()); 
        database same=new database(); 
          
        for(int i=0;i<dbrc.filas.Count;i++)  
        {              
            if(_<IMyTimerBlock>(dbrc.GetDataString(i,0)).IsCountingDown||same.existEnColumnas(0,dbrc.GetDataString(i,0)))              
            {  
                //TimerBlock | (*)ObjetName, TypeObjet|Actions| IP  
                string msg="msg#"+dbrc.GetDataString(i,3).Replace('-','.')+"|ControlRemote|"+dbrc.GetDataString(i,1)+"|"+dbrc.GetDataString(i,2)+"\n"; 
                _<IMyTextPanel>(n.Components[4]).WritePublicText(msg,true); 
                if(_<IMyTimerBlock>(dbrc.GetDataString(i,0)).IsCountingDown) 
                { 
                      
                    same.filas.Add(dbrc.GetDataString(i,0)); 
                    _<IMyTimerBlock>(dbrc.GetDataString(i,0),"Stop"); 
                }  
                 
                  
            }  
        }  
          
    }  
}        
  
