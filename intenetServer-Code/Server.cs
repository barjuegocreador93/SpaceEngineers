 //
//Server nodo=new Server( string TimerBlocksGrups,string MainDispaly,bool SaveUsers=false, string IP="0-", 
//string DatabaseIPs="DBplaces",string DatabaseUsers="DBusers",  string DatabaseRam="DBram" )



Server nodo=new Server("Timer Block nodo 0","Display nodo 0",false);


 
void Main()      
{        
    ServerMain(ref nodo);  
}    

bool IsConnected(IMyLaserAntenna antenna) 
    {       
    return antenna.DetailedInfo.Split('\n')[2].Contains("Connected to"); 
    }  



   IMyTerminalBlock capturar_objeto(string _obj)          
{          
return GridTerminalSystem.GetBlockWithName(_obj);         
}         
         
         
         
void cambiarAccionObjeto(string _objeto, string _accion)          
{         
IMyTerminalBlock objeto =capturar_objeto(_objeto);          
ITerminalAction  accion = objeto.GetActionWithName(_accion);                                                                                                                    
accion.Apply(objeto);         
}          
         
bool Filter_Method_Laser_Antenna (IMyTerminalBlock block)      
    {      
    IMyLaserAntenna la = block as IMyLaserAntenna;      
    return la != null;      
    }             
string _s(string name, Func<IMyTerminalBlock, bool> collect = null)
{
    List<IMyTerminalBlock> list=new List<IMyTerminalBlock>();
    GridTerminalSystem.SearchBlocksOfName(name,list,collect);
    return list[0].CustomName;
}         
         
T _<T>(string name, string action="")where T : class          
{              
    if(action.Length != 0) 
     { 
            string[] actions=action.Split(','); 
            foreach(string act in actions) 
                cambiarAccionObjeto(name,act); 
     } 
return  capturar_objeto(name) as  T;           
}	 
           
 
 
void __<T>(string name, string action)where T : class  
{ 
        string[] names=name.Split(','); 
        foreach(string n in names){ 
        for(int i=1;exist(n+i.ToString());i++) 
         { 
                _<T>(n+i.ToString(),action); 
          }} 
} 
      
void __<T>(string name, ref List<string> blocks)where T : class   
{  
        string[] names=name.Split(',');  
        foreach(string n in names){  
        for(int i=1;exist(n+i.ToString());i++)  
         {                  
                blocks.Add(n+i.ToString());  
          }}  
}      
         
bool exist(string nombre_objeto)         
{         
    if(capturar_objeto(nombre_objeto)==null)         
    return false;         
    return true;         
}         
 
void mostrar(string text,string display, bool pub=true, bool show=true) 
{ 
     if(pub) _<IMyTextPanel>(display).WritePublicText(text); 
        else _<IMyTextPanel>(display).WritePrivateText(text);
     if(show)_<IMyTextPanel>(display). ShowPublicTextOnScreen(); 
} 
 
 
int teclado(string keyup,string keydown,string keycheck) 
{ 
  if(_<IMyTimerBlock>(keyup).IsCountingDown) 
  {cambiarAccionObjeto(keyup,"Stop");return 1; }
  if(_<IMyTimerBlock>(keydown).IsCountingDown) 
  {cambiarAccionObjeto(keydown,"Stop");return 2; }
  if(_<IMyTimerBlock>(keycheck).IsCountingDown) 
  {cambiarAccionObjeto(keycheck,"Stop");return 3; }  
  return 0;       
} 
    
class database     
{     
    	public List<string> filas;    
	   public List<string> columnas;     
	    
	   //@param columna="type1,type2,typeN"    
	   //@param db="fila1,fila2,filaN"    
	   //string filaN="colum1|column2|columN"    
	   public database(string db="null", string columna="null")     
	   {  
           filas=new List<string>();  
		   columnas=new List<string>();  
        if(columna!="null"&&columna.Length!=0){  
	       	string[] aux1=columna.Split(',');     
		      foreach (string item in aux1)     
		      {      
			         columnas.Add(item);     
		      }   }  
        if(db!="null"&&db.Length!=0){  
		      string[] aux2=db.Split('\n');     
		      foreach (string item in aux2)     
		      {       
			         filas.Add(item);     
		      }   }  
	   }     
    	//@param fila="colum1|column2|columN"    
    	public void Addfila(string fila)     
	   {     
	       filas.Add(fila);     
	   }    
    public void Setfila(int Indexfila, string fila)
    {
        if(Indexfila>=0&&Indexfila<filas.Count)
        filas[Indexfila]=fila;         
    }
    public void SetColumn(int indexfila, int indexColum, string data)
    {
        if(indexfila>=0&&indexfila<filas.Count)
        {
            string[] fila=filas[indexfila].Split('|');
            if(indexColum>=0&&indexColum<fila.Length)
            {
                fila[indexColum]=data;
                filas[indexfila]="";
                foreach(string d in fila)
                        {
                            filas[indexfila]+=d;
                            if(d!=fila[fila.Length-1])filas[indexfila]+="|";
                        }
            }
        }
    }    
	   public int GetDataInt(int Indexfila, int IndexCol)   
	   {    
		     
				           int aux = Int32.Parse(GetDataString( Indexfila,IndexCol));		         
		      return aux;    
	   }    
	   public float GetDataFloat(int Indexfila, int IndexCol)    
	   {     
		      float aux;	      
				            aux = float.Parse(GetDataString( Indexfila,IndexCol ));        
		      return aux;     
	   }    
 public string GetDataString(int Indexfila, int IndexCol)     
	   {      
		      string aux="";      
		      if(Indexfila>=0&&Indexfila<filas.Count)      
		      {      
			         string[] auxColum=filas[Indexfila].Split('|');      
			         int cont=0;      
			         foreach(string item in auxColum)      
			         { 
            if(cont==IndexCol)
                return item;     
				            cont++;      
			         }      
			            
			      
		      }	      
		      return aux;      
	   }    
   
    	public string Save()    
	   {    
		      string text="";    
		      for(int i = 0;i<filas.Count;i++)    
			      {    
				        text+=filas[i];
        if(i<filas.Count-1)text+="\n";   
			      }    
		      return text;    
	   } 
    public bool existEnColumnas(int columna, string var)
    {
        for(int i=0;i<filas.Count;i++)
        {
            if(GetDataString(i,columna)==var)
                return true;
        }
        return false;
    } 
     public string[] FilaForColumna(int columna, string var)
        {
            for(int i=0;i<filas.Count;i++)
             {
                if(GetDataString(i,columna)==var)
                    return filas[i].Split('|');    
             } 
                string[] nu={"null"};             
              return nu;           
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
      private  IMyTextPanel display;  
       
      public menu(string titulo,string aditionaltext)  
      {  
            title=titulo;  
            addtext=aditionaltext;            
            options=new List<string>();  
            sig=new List<int>();  
            cursorPosition=1;                          
      }        
	public void addOption(string name, int menu_sig)  
      {  
            options.Add(name);  
            sig.Add(menu_sig);  
      }  
      public int teclado(int teclaon)  
      {  
            if(teclaon==1)  
            {  
                  tecla=1;  
                  if(cursorPosition>1)     
                  cursorPosition--;  
                  return 1;                     
            }else  
            if(teclaon==2)  
            {  
                  tecla=2;  
                  if(cursorPosition<options.Count)     
                  cursorPosition++;  
                  return 2;                      
            }else  
            if(teclaon==3)  
            {  
              return tecla=3;             
                   
            }else  
            if(teclaon==4)  
            {  
               return tecla=4;                   
            }  
            return 0;  
      }  
      public string mostrar()  
      {  
            string text=title+"\n";  
            text+=addtext+"\n";  
            for(int i=0;i<options.Count;i++)  
                  {  
                        if(cursorPosition-1==i)  
                        text+=" > "+(i+1).ToString()+". "+options[i]+"\n";  
                        else text+="   "+(i+1).ToString()+". "+options[i]+"\n";  
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
            return sig[cursor_position-1];  
      }  
        
}   
  
class interfaz  
{  
      public List<menu> ifcs;       
      private int ifact;
      private bool ready=true;  
      public interfaz()  
      {  
         ifcs=new List<menu>();  
         ifact=0;     
      }  
      public void addMenu(string titulo,string aditionaltext)  
      {  
            menu aux=new menu(titulo,aditionaltext);  
            ifcs.Add(aux);  
      }  
      public void addOptionIn(int index, string name, int sig)  
      {  
            ifcs[index].addOption(name,sig);  
      }  
      public List<int> run(int teclaon)//return 0=tecla 1=cursorposition 2=ifact  
      {  
            List<int> aux=new List<int>();           
            aux.Add(ifcs[ifact].teclado(teclaon));  
            aux.Add(ifcs[ifact].GetCusrorPosition());  
            aux.Add(ifact);  
            if(aux[0]==3)  
            {  
                  ifact=ifcs[ifact].GetSig(aux[1]);  
            }  
             
            return aux;  
      } 
      public int GetIfact() 
      { 
            return ifact; 
      }
      public void SetIfact(int index)
        {
                if(index>=0&&index<ifcs.Count)
                ifact=index;
        }
      public void makeUsingDataBase(string db)
        {
            if(ready) 
            { 
                database dbid=new database(db); 
                for(int i=0;i<dbid.filas.Count;i++) 
                { 
                string[] colum=dbid.FilaForColumna(0,dbid.GetDataString(i,0));
                string[] text=colum[1].Split('#');
                colum[1]="";
                foreach(string n in text)
                        colum[1]+=n+"\n";
                addMenu(colum[0],colum[1]); 
                foreach(string options in colum) 
                        { 
                            if(options!=colum[0]&&options!=colum[1]) 
                            { 
                                string[] option=options.Split(';');                               
                                int sig=int.Parse(option[1]); 
                                addOptionIn(i,option[0],sig); 
                            }    
                        }  
                }
                ready=false;              
            }
        }  
}  

class Server
{    
    public List<string> Components=new List<string>();
    public interfaz id = new interfaz();
    public string IP;
    public List<string> msg=new    List<string>(); 
    public bool ready=true;
    public bool SaveUsersHere=false;
    public Server(string timerblocks, string textpanel,bool SaveUser=false, string ip="0-",
    string DBplaces="DBplaces",string DBusers="DBuser", 
    string DBram="DBram")
    {
        Components.Add(timerblocks);//0 keys and time
        Components.Add(textpanel);//1 visual
        Components.Add(DBplaces);//2 save place : antennas
        Components.Add(DBusers);//3 save user
        Components.Add(DBram);//4 processes
        SaveUsersHere=SaveUser;
        IP=ip;
    }
    public void SupliteMSGAntenna(string dbplace)
    {
           if(ready){ database p=new database(dbplace);
            for(int i=0;i<p.filas.Count;i++)
            msg.Add("");
                ready=false;
            }
    }    
    
}
string ServerMenu()
{
    string menus="Main Menu|Use this panel to create a new #IP: |Create Point;1 \n";   
    menus+="Register-id|Are you sure to create a new point?|Yes;0|No;0"; 
           
    return menus; 
}

void Server_Regist_User(ref database m, ref int i,ref Server nodo)
{
   string IdUser=m.GetDataString(i,1);
   string name=m.GetDataString(i,2);
   string password=m.GetDataString(i,3);
   string IPPersonalDatabaseInbox=m.GetDataString(i,4);
   string NamePersonalDatabaseInbox=m.GetDataString(i,5);
   string status=m.GetDataString(i,6);
   string IPmade=m.GetDataString(i,7);
   string Device=m.GetDataString(i,8);
   string serial=m.GetDataString(i,9);
    
   
    if(status=="quest")
    {
         database user=new database(_<IMyTextPanel>(nodo.Components[3]).GetPublicText());
         if(!user.existEnColumnas(0,IdUser))
         {
            status="logOut";
            user.filas.Add(IdUser+"|"+name+"|"+password+"|"+IPPersonalDatabaseInbox+"|"+NamePersonalDatabaseInbox
            +"|"+status+"|"+IPmade+"|"+Device);
            
            _<IMyTextPanel>(nodo.Components[3]).WritePublicText(user.Save());
          m.filas.RemoveAt(i);
          m.filas.Add("msg#"+IPmade+"|"+nodo.IP.Replace('-','.')+"|"+ 
                Device+"|regist,true|"+serial);
          _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());    
         }else
          {
            m.filas.RemoveAt(i); 
            m.filas.Add("msg#"+IPmade+"|"+nodo.IP.Replace('-','.')+"|"+
                Device+"|regist,false|"+serial);  
          _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());    
          }  
       
    }
    else
    {
        m.filas.RemoveAt(i);
        _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());  
    } 
      
        
}
bool Server_SendMSG(string url, ref database p, int i, ref database m)
{
            if(!Server_PortsEquals(url,ref p))  
            {  
                string ipUrl=Server_WayMsg(url,ref p);  
                if(ipUrl!="null"){  
                string antenna=_s(ipUrl,Filter_Method_Laser_Antenna);  
                _<IMyTerminalBlock>(antenna).SetCustomName(ipUrl+m.filas[i]);  
                antenna=_s(ipUrl,Filter_Method_Laser_Antenna); 
                _<IMyLaserAntenna>(antenna,"OnOff_Off");  
                _<IMyLaserAntenna>(antenna,"OnOff_On");                 
                }  
                m.filas.RemoveAt(i);  
                _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());
                return true;  
            }
    return false;

}

bool Server_PortsEquals(string IP, ref database p)
{
        for(int i=0;i<p.filas.Count;i++)
            if(p.GetDataString(i,0)==IP)return true;
    return false;
}

string Server_WayMsg(string url, ref database p)
{
      for(int i=0;i<p.filas.Count;i++)
        {
            string result;
            if(url.Length >= p.GetDataString(i,0).Length){
            string surl=url.Remove(p.GetDataString(i,0).Length-2);
            string sip=p.GetDataString(i,0).Remove(p.GetDataString(i,0).Length-2);
            if(surl!=sip)result=p.GetDataString(0,0);
            else result=url.Remove(p.GetDataString(i,0).Length);
            }else result=p.GetDataString(0,0);
                                
            if(Server_PortsEquals(result,ref p))return result;
        }
        return "null";  
}   

void ServerMain(ref Server nodo)
{
    nodo.SupliteMSGAntenna(_<IMyTextPanel>(nodo.Components[2]).GetPublicText());
    nodo.id.makeUsingDataBase(ServerMenu()); 
    mostrar(nodo.id.ifcs[nodo.id.GetIfact()].mostrar(),nodo.Components[1]);     
    List<int> moves=nodo.id.run(teclado(nodo.Components[0]+" 1",nodo.Components[0]+" 2",nodo.Components[0]
    +" 3"));
    if(moves[0]==3)
    {
            if(moves[2]==1&&moves[1]==1)
            {
                    database place=new database(_<IMyTextPanel>(nodo.Components[2]).GetPublicText());
                    if(place.filas.Count!=0)
                    {
                        string[] ip=place.GetDataString(place.filas.Count-1,0).Split('-');
                        int i=int.Parse(ip[ip.Length-2])+1;
                        string newIp="";
                        for(int x=0;x<ip.Length-2;x++)
                                newIp+=ip[x]+"-";
                        newIp+=i+"-";
                        newIp+="|"+newIp+"0-";
                        place.filas.Add(newIp);
                        _<IMyTextPanel>(nodo.Components[2]).WritePublicText(place.Save());
                        nodo.msg.Add("");
                    }else
                    {
                       place.filas.Add(nodo.IP+"|"+nodo.IP+"0-");
                        _<IMyTextPanel>(nodo.Components[2]).WritePublicText(place.Save());
                        nodo.msg.Add("");                       
                    }
            }
    }
                                    //Save Message in a RAM
    database p=new database(_<IMyTextPanel>(nodo.Components[2]).GetPublicText());
    database m=new database(_<IMyTextPanel>(nodo.Components[4]).GetPublicText());
    for(int i=0;i<p.filas.Count;i++)
    {
        string antenna=_s(p.GetDataString(i,0),Filter_Method_Laser_Antenna);
        
        if(IsConnected(_<IMyLaserAntenna>(antenna)))
        {
        string antInfo=_<IMyTerminalBlock>(antenna).DetailedInfo;
        string[] antenna2=antInfo.Split('\n')[2].Remove(0,12).Split('-');
        string message=antenna2[antenna2.Length-1];
        if(nodo.msg[i]!=message){m.filas.Add(message);nodo.msg[i]=message;}     }  
    }
    _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());

    
    for(int i=0;i<m.filas.Count;)
    {
        
        string typeRq=m.GetDataString(i,0).Split('#')[0];
        string url=m.GetDataString(i,0).Split('#')[1].Replace('.','-');
        switch(typeRq)
        {
            case "regist":
            if(nodo.SaveUsersHere&&Server_PortsEquals(url,ref p))Server_Regist_User(ref m, ref i,ref nodo);
            else Server_SendMSG(url, ref p,i,ref m);
            break;  
            case "msg": if(!Server_SendMSG(url, ref p,i,ref m))
                {
                    if(p.existEnColumnas(0,m.GetDataString(i,2).Replace('.','-')))
                    {
                        string[] msg= m.GetDataString(i,3).Split(',');
                        if(msg.Length==2){
                        switch(msg[0])
                        {
                             case "Quest":
                                    if(msg[1]=="saveUser")
                                    {
                                        string ans;                               
                                        if(nodo.SaveUsersHere)ans="true";
                                        else ans="false";
                                        if(m.GetDataString(i,1).Split(',').Length==2)
                                        {
                                            m.filas.Add("msg#"+m.GetDataString(i,1).Split(',')[1]+"|"+
                                            p.GetDataString(0,0).Replace('-','.')+"|"+m.GetDataString(i,1).Split(',')[0]+"|saveUser,"
                                            +ans+"|"+m.GetDataString(i,4));
                                            m.filas.RemoveAt(i);
                                            _<IMyTextPanel>(nodo.Components[4]).WritePublicText(m.Save());
                                        }else i++;
                                    }
                                break;
                                default: i++;break;
                        }
                    }else i++;
                    } else            
                    i++;
                }
            break;
            default: i++;break;
            
        }
        
    }
    _<IMyTimerBlock>(nodo.Components[0]+" 4","Start");
    
}
 
 
