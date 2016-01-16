SystemOperative alfa=new SystemOperative();
sdtp  security = new sdtp();

 
void Main()      
{
    sdtp_Main(ref security,"DoorDisplayAkm");
    SO_main(ref alfa);    
}
 



//FILTROS: 
bool Filter_Method_Laser_Antenna (IMyTerminalBlock block)       
    {       
    IMyLaserAntenna la = block as IMyLaserAntenna;       
    return la != null;       
    }
bool Filter_Method_Text_Panel (IMyTerminalBlock block)        
    {        
    IMyTextPanel la = block as IMyTextPanel;        
    return la != null;        
    } 
//-- 
           
//FUNCIONES BASICAS:          
//Buscar objeto por nombre y filtro              
string _s(string name, Func<IMyTerminalBlock, bool> collect = null) 
{ 
    List<IMyTerminalBlock> list=new List<IMyTerminalBlock>(); 
    GridTerminalSystem.SearchBlocksOfName(name,list,collect); 
    return list[0].CustomName; 
} 
 
 
 
//capturar objeto u objetos y/o que realicen una accion de una clase T  de una clase T ejemplo <IMyDoor> devolviendo el obejto         
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
            
  
//hacer que uno o varios grupos de objetos realicen una opcion  de una clase T ejemplo <IMyDoor> sin devolver nada 
void __<T>(string name, string action)where T : class   
{  
        string[] names=name.Split(',');  
        foreach(string n in names){  
        for(int i=1;exist(n+i.ToString());i++)  
         {  
                _<T>(n+i.ToString(),action);  
          }}  
}  
//guardar en una lista referenciada; uno o varios grupos de obejtos de una clase T ejemplo <IMyDoor> sin devolver nada        
void __<T>(string name, ref List<string> blocks)where T : class    
{   
        string[] names=name.Split(',');   
        foreach(string n in names){   
        for(int i=1;exist(n+i.ToString());i++)   
         {                   
                blocks.Add(n+i.ToString());   
          }}   
}       
//pregunta si un objeto existe          
bool exist(string nombre_objeto)          
{          
    if(capturar_objeto(nombre_objeto)==null)          
    return false;          
    return true;          
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
 
//permite mostrar un texto en un display, true si en la parte publica ! privada, true para mostrar el mensaje publico  
void mostrar(string text,string display, bool pub=true, bool show=true)  
{  
     if(pub) _<IMyTextPanel>(display).WritePublicText(text);  
        else _<IMyTextPanel>(display).WritePrivateText(text); 
     if(show)_<IMyTextPanel>(display). ShowPublicTextOnScreen();  
}  
  
//permite saber si un grupo de timer blocks se inician y devuelve un valor  
int teclado(string keygrup)  
{  
	List<string> keys=new List<string>(); 
	__<IMyTimerBlock>(keygrup,ref keys); 
	for(int i=0;i<keys.Count;i++) 
	{ 
	if(_<IMyTimerBlock>(keys[i]).IsCountingDown)  
  	{_<IMyTimerBlock>(keys[i],"Stop");return i+1; }	 
	} 
  return 0;        
} 
 
//OBJETOS: 
 
//Obejto de base de datos     
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
    public int IndexFilaOfColumna(int columna, string var)
      {  
            for(int i=0;i<filas.Count;i++)  
             {  
                if(GetDataString(i,columna)==var)  
                    return i;      
             }                              
             return -1;             
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
    public string[] FilaForColumna(int index_fila)  
    {  
        if(index_fila>=0&&index_fila<filas.Count)  
        {  
  
            return filas[index_fila].Split('|');  
        }  
        string[] nu= {"null"};  
        return nu;  
    }  
   
}      
        
//Objeto de menu 
class menu   
{   
	private int cursorPosition;   
	private int tecla;   
      private string title;   
      public string addtext;   
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
 
//Obejto de inetrfaz   
class interfaz   
{   
      public List<menu> ifcs;        
      public int ifact; 
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
      public void SetAditionalText(int i, string text)
       {
            ifcs[i].addtext=text;
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

 class sdtp  
{  
    public List<string[]> doors=new List<string[]>() ;  
    public List<string[]> panels= new List<string[]>() ;  
    public List<string> passwords=new List<string>() ;  
    public List<bool> open=new List<bool>();  
    public List<int> time=new List<int>();  
    public bool ready=true;  
    public sdtp() {}  
    public void apliSecurty(string sdoors, string spanels, string pass)  
    {  
        string[] ssdoors= sdoors.Split(',');  
        string[] sslcd= spanels.Split(',');  
        doors.Add(ssdoors);  
        panels.Add(sslcd);  
        passwords.Add(pass);  
        open.Add(true);  
        int t=0;  
        time.Add(t);  
    }  
    public void clear()  
    {  
        doors.Clear();  
        panels.Clear();  
        passwords.Clear();  
        time.Clear();  
    }  
    public void apliSecurtyByDataBase(string db)  
    {  
        if(ready)  
        {  
            clear();  
            database securty= new database(db);  
            for(int i=0; i<securty.filas.Count; i++)  
            {  
                string[] sec=securty.FilaForColumna(i);  
                apliSecurty(sec[0],sec[1],sec[2]);  
            }  
            ready=false;  
        }  
    }  
  
  
}  
  
void sdtp_passOn (ref sdtp lcds, int i)  
{  
    int x=0;  
    foreach(string lcd in lcds.panels[i])  
    {  
        if(_<IMyTextPanel>(lcd).GetPublicTitle()==lcds.passwords[i])  
            x++;  
        if(_<IMyTextPanel>(lcd).GetPublicTitle()=="close")  
        {  
            foreach(string lcd2 in lcds.panels[i])  
            _<IMyTextPanel>(lcd2).WritePublicTitle("Public Title");  
            lcds.open[i]=false;  
            lcds.time[i]=0;  
            return;  
        }  
    }  
    if(x!=0)  
        {  
            lcds.open[i]=true;  
            if(lcds.time[i]<2)lcds.time[i]=2;  
        }  
    else  
    {  
        lcds.open[i]=false;  
        if(lcds.time[i]>2)lcds.time[i]=0;  
    }  
  
  
}  
  
void sdtp_close(ref sdtp door, int i)  
{  
    int x=0; 
     
    foreach(string dr in door.doors[i])  
    {if(dr[0]=='*') 
    { 
       string setdr=dr.Substring(1); 
        List<string> ldrs=new List<string>();  
        __<IMyDoor>(setdr,ref ldrs); 
        for(int j=0;j<ldrs.Count;j++) 
        { 
           if(!_<IMyDoor>(ldrs[j]).Open&&door.open[i]&&door.time[i]==0) 
            x++;  
        }       
    } 
    else 
    if(!_<IMyDoor>(dr).Open&&!door.open[i]&&door.time[i]==0)  
        x++;}  
    if(x!=0)  
    {  
        foreach(string lcd in door.panels[i])  
        _<IMyTextPanel>(lcd).WritePublicTitle("close");  
        door.open[i]=false;  
        door.time[i]=0;  
    }  
}  
  
  
void sdtp_doors(string[] doors, string action="Open_Off")  
{  
    foreach(string dr in doors)  
    {  
    if(dr[0]=='*')  
    {  
        string setdr=dr.Substring(1);         
        __<IMyDoor>(setdr,action);  
           
    }else   _<IMyDoor>(dr,action);  
    }  
}  
  
void sdtp_Main(ref sdtp security, string db)  
{  
    if(_<IMyTextPanel>(db).GetPublicTitle()=="read")  
    {  
        security.ready=true;  
        _<IMyTextPanel>(db).WritePublicTitle("Public Title");  
    }  
    security.apliSecurtyByDataBase(_<IMyTextPanel>(db).GetPublicText());  
    for(int i=0; i<security.panels.Count; i++)  
    {  
  
        sdtp_passOn(ref security, i);  
        if(security.open[i])  
        {  
            if(security.time[i]==2)  
            {  
                sdtp_doors(security.doors[i],"OnOff_On");  
                security.time[i]++;  
            }  
            else if(security.time[i]<=4)  
            {  
                sdtp_doors(security.doors[i],"Open_On");  
                security.time[i]++;  
  
            }  
            else 			security.time[i]=0;  
            if(		security.time[i]==0) sdtp_close(ref security,i);  
  
        }  
        else  
  
            if(security.time[i]>=2)  
            {  
                sdtp_doors(security.doors[i],"OnOff_Off");  
  
  
            }  
            else  
            {  
                security.time[i]++;  
                sdtp_doors(security.doors[i]);  
            }  
  
    }  
  
} 


class SystemOperative
{
    public interfaz shell = new interfaz();
    public database Config;
    public string NameOfDivece;
    public bool online; 
    public string user; 
    public string ip;
    public bool ready=true;
    public string serial;
    public List<string> externals=new List<string>();    
    public string DBram;
    public List<string> QuestRegister= new List<string>();
    public SystemOperative(string nameTimerBlockGrup="key",string nameOfDivece="Akm-0",
    string nameDisplayChat="DChat1x2", string nameDisplayInput="Input", string nameDBcache="DBcache",
    string nameDBram="DBram", string nameDBConfig="DBConfig")
    {
        NameOfDivece=nameOfDivece;
        externals.Add(nameTimerBlockGrup);//externals[0] TimerBlockGrup
        externals.Add(nameDisplayChat);//externals[1] DisplayChat
        externals.Add(nameDisplayInput);//externals[2] DisplayInput 
        externals.Add(nameDBcache);//externals[3] DBcache
        externals.Add(nameDBConfig);//externals[4] DBConfig
        DBram=nameDBram;             
    }
    public string Interfaz()
    {//meves[2]=
//0
        string menus="Main Menu "+NameOfDivece+"|Welcome to divece#Please Select one: |Register;1|Sign in;5|Config;7 \n";
//1
        menus+="Register->ID|write your ID in the public title:|Next;2|Back;0 \n";
//2
        menus+="Register->Password|write your Password in the public title:|Next;3|Back;1 \n";
//3
        menus+="Register->DBChat|write your DBchat name in the public title:|Next;4|Back;2 \n";
//4
        menus+="Register->MSG|ServerMessage:|Finish;0 \n";
//5
        menus+="Sign in-ID|write your ID in the public title:|Next;6|Back;0 \n";
//6
        menus+="Sign in-Pasword|write your Password in the public title:|Next;0|Back;0 \n";
//7     
        menus+="Config|Configurations:|Conective;8|Local IP;10|Back;0\n";
//8
        menus+="Config->Conective|Write the IP how can save users: |Start;9|Back;7\n";
//9 
        menus+="Config->Conective->msg|Message: |Ok;7|Try Again;8|Main Menu;0\n";
//10 
        menus+="Config->Local IP|Write a name of Laser Antenna(IP): |Ok;7|Main Menu;0";
            
        return menus;   
    }
}
string randomstr()
 {
        Random a=new Random();
        int num= a.Next(0,10);
        return num.ToString();
 }   
void SO_Config(ref SystemOperative alfa, ref List<int> moves)
{  
        alfa.Config= new database(_<IMyTextPanel>(alfa.externals[4]).GetPublicText());
        alfa.ip=alfa.Config.FilaForColumna(0,"IP")[0]!="null"?alfa.Config.FilaForColumna(0,"IP")[1]:"null";
        if(moves[0]==3)
        {
            
            if(moves[1]==1&&moves[2]==8)
            {
                    string serial;
                    do
                    serial=randomstr()+randomstr()+randomstr()+randomstr();
                    while(serial==alfa.serial);
                    alfa.serial=serial;
                    string ip=_<IMyTextPanel>(alfa.externals[2]).GetPublicTitle().Replace('-','.');
                    if(alfa.ip!="null")
                    alfa.Config.filas.Add("IPServerUser|"+ip);
                    else alfa.Config.SetColumn(alfa.Config.IndexFilaOfColumna(0,"IPServerUser"),1,ip);
                    if(alfa.Config.FilaForColumna(0,"IP")[0]!="null"){
                    alfa.ready=true;
                    database ram=new database(_<IMyTextPanel>(alfa.DBram).GetPublicText());                    
                    ram.filas.Add("msg#"+ip+"|"+alfa.NameOfDivece.Replace('-','.')+","+
                    alfa.Config.FilaForColumna(0,"IP")[1].Replace('-','.')+"|"+ip+"|Quest,saveUser"+"|"+serial);
                    _<IMyTextPanel>(alfa.DBram).WritePublicText(ram.Save());}else alfa.shell.ifact=10;
                    
            }
            if(moves[1]==1&&moves[2]==10)
            {
                string ip=_<IMyTextPanel>(alfa.externals[2]).GetPublicTitle().Replace('-','.');
                alfa.ip=ip;
                if(alfa.Config.FilaForColumna(0,"IP")[0]=="null") 
                    alfa.Config.filas.Add("IP|"+ip);
                else alfa.Config.SetColumn(alfa.Config.IndexFilaOfColumna(0,"IP"),1,ip);
                _<IMyTextPanel>(alfa.externals[4]).WritePublicText(alfa.Config.Save());
            }
        }
        if(moves[2]==9)
        {
            if(alfa.ready)alfa.shell.SetAditionalText(9,"Plese wait a moment\nWe try to connectiv to:\n ");
            database ram=new database(_<IMyTextPanel>(alfa.DBram).GetPublicText());
            for(int i=0;i<ram.filas.Count&&alfa.ready;)
            {
                if(ram.GetDataString(i,2)==alfa.NameOfDivece.Replace('-','.') && ram.GetDataString(i,4)
                ==alfa.serial)
                {
                    string[] msg=ram.GetDataString(i,3).Split(',');
                    switch(msg[0])
                    {
                        case "saveUser":
                            if(msg[1]=="true")
                            {
                                alfa.shell.SetAditionalText(9,"Conetive Succses!");
                                string ip=_<IMyTextPanel>(alfa.externals[2]).GetPublicTitle().Replace('-','.'); 
                                if(alfa.ip!="null") 
                                alfa.Config.filas.Add("IPServerUser|"+ip); 
                                else alfa.Config.SetColumn(alfa.Config.IndexFilaOfColumna(0,"IPServerUser"),1,ip);  
                                _<IMyTextPanel>(alfa.externals[4]).WritePublicText(alfa.Config.Save());
                                alfa.ready=false;                                
                            }else
                            {
                                alfa.shell.SetAditionalText(9,"The server can't keep a data! plese choese other"); 
                                alfa.Config.filas.RemoveAt(alfa.Config.IndexFilaOfColumna(0,"IPServerUser"));
                                _<IMyTextPanel>(alfa.externals[4]).WritePublicText(alfa.Config.Save());
                            }
                            ram.filas.RemoveAt(i);
                            _<IMyTextPanel>(alfa.DBram).WritePublicText(ram.Save());
                        break;                       
                        default: i++; 
                        break;
                    }
                }
                else i++;
            }
        
    }
}
void SO_main(ref SystemOperative alfa)
{
    
    alfa.shell.makeUsingDataBase(alfa.Interfaz()); 
    mostrar(alfa.shell.ifcs[alfa.shell.GetIfact()].mostrar(),alfa.externals[2]); 
    List<int> moves=alfa.shell.run(teclado(alfa.externals[0]));
    SO_Config(ref alfa, ref moves);
    


    _<IMyTimerBlock>("time","Start");
}
