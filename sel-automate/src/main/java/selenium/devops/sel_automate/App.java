package selenium.devops.sel_automate;

public class App 
{
	//private fields
	
	private static String op;
	private static String loginUrl;
	private static String username;
	private static String password;
	private static String homepage;
	private static String file;
	private static String selHub;
	private static String browser;
	private static String conf;
	
    public static void main( String[] args ) throws InterruptedException
    {
    	SeleniumDriver selDriver = new SeleniumDriver();
    	
    	for(String arg:args)
    		 if(isOption(arg)) {
    			 if(isValid(arg))
    				 op = arg;
    		 } else {
    			 if(!op.isEmpty()) {
    				 assignValue(arg);
    				 op = "";
    			 }
    		 }
    	System.out.println("-----------------------------------------------------");
    	System.out.println(" 		CONNECTING TO SELENIUM HUB. . . ");
    	System.out.println("-----------------------------------------------------");
    	System.out.println("Selenium Hub Server: " + selHub);
    	System.out.println("Browser: " + browser);
    	
    	if(!selDriver.connectToHub(selHub, browser))
    		System.exit(1);
    	
    	System.out.println("-----------------------------------------------------");
    	System.out.println(" 		LOGGING IN. . .");
    	System.out.println("-----------------------------------------------------");
    	System.out.println("Login URL: " + loginUrl);
    	if(!selDriver.login(loginUrl, username, password))
    	{
    		selDriver.dispose();
    		System.exit(2);
    	}
    	
    	System.out.println("-----------------------------------------------------");
    	System.out.println(" 		APPLYING CONFIGURATIONS. . .");
    	System.out.println("-----------------------------------------------------");
    	System.out.println("Configuration file: " + conf);
    	
    	if(!selDriver.run(conf, file, homepage))
    	{
    		System.out.println("-----------------------------------------------------");
    		System.out.println(" 		CONFIGURATION FAILED!");
    		System.out.println("-----------------------------------------------------");
    		selDriver.dispose();
    		System.exit(2);
    	}
    	
    	System.out.println("-----------------------------------------------------");
		System.out.println(" 		CONFIGURATION SUCCESS!");
		System.out.println("-----------------------------------------------------");
		selDriver.dispose();
    }
    
  
    private static boolean isOption(String arg) {
    	if(arg.charAt(0) == '-') return true;
    	return false;
    }
    
    private static boolean isValid(String arg) {
    	if( arg.equals("-l") || arg.equals("-u") || arg.equals("-p") || arg.equals("-h") || arg.equals("-f") ||
    			arg.equals("-s") || arg.equals("-b") || arg.equals("-c"))
    		return true;
    	return false;
    }
    
    private static void assignValue(String arg) {
    	if (op.contentEquals("-s")) 	 selHub   = arg;
    	else if (op.contentEquals("-b")) browser  = arg;
    	else if (op.contentEquals("-l")) loginUrl = arg;
    	else if (op.contentEquals("-u")) username = arg;
    	else if (op.contentEquals("-p")) password = arg;
    	else if (op.contentEquals("-h")) homepage = arg;
    	else if (op.contentEquals("-f")) file	  = arg;
    	else if (op.contentEquals("-c")) conf	  = arg;
    }
    
    private static void printHelp() {
    	System.out.println("Usage:\t\tSYNTAX: *.jar [OPTIONS]");
    }
}