
//You need database and interfaz copy tu run this


//main example:
sdtp  security = new sdtp(); 
void Main() 
{ 
 
    sdtp_Main(ref security,"DB_DoorsSecurity"); 
    _<IMyTextPanel>("MyDisplay2").WritePublicText(_<IMyLaserAntenna>("LaserAntenna").DetailInfo);
   
 
    _<IMyTimerBlock>("mytime","Start"); 
} 


//Doors Securty system Advanced 
 
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
