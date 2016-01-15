void Main()
{
  
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
