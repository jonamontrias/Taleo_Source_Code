package selenium.devops.sel_automate;


import selenium.devops.data.*;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

import org.apache.commons.io.FileUtils;
import org.openqa.selenium.*;
import org.openqa.selenium.remote.DesiredCapabilities;
import org.openqa.selenium.remote.RemoteWebDriver;
import org.openqa.selenium.support.ui.*;

public class SeleniumDriver {

	private DesiredCapabilities capability;
	private WebDriver driver;
	private String previous = "";
	private Pages website;
	private Page current;
	private WebDriverWait wait;
	
	public boolean connectToHub(String selUrl, String browser) {
		
		if(browser.contentEquals("firefox"))
			capability = DesiredCapabilities.firefox();
		else
			capability = DesiredCapabilities.chrome();
		
		try {
			driver = new RemoteWebDriver(new URL(selUrl), capability);
			wait = new WebDriverWait(driver, 30);
			System.out.println("Connection Successful!");
			return true;
		} catch (Exception e) {
			System.out.println("Connection failed!");
			e.printStackTrace();
		}
		return false;
	}
	
	public boolean login(String loginUrl, String username, String password) {
		try {
			driver.get(loginUrl);
			driver.findElement(By.id("dialogTemplate-dialogForm-content-login-name1")).clear();
			driver.findElement(By.id("dialogTemplate-dialogForm-content-login-name1")).sendKeys(username);
			driver.findElement(By.name("dialogTemplate-dialogForm-content-login-password")).clear();
			driver.findElement(By.name("dialogTemplate-dialogForm-content-login-password")).sendKeys(password);
			driver.findElement(By.id("dialogTemplate-dialogForm-content-login-defaultCmd")).click(); 
			System.out.println("Login successful!");
			return true;
		} catch (Exception e) {
			System.out.println("Login failed!");
			e.printStackTrace();
		}
		return false;
	}

	public boolean run(String conf, String file, String homepage) throws InterruptedException {
		//load and read configuration file
		List<String[]> configs =  readInputfile(conf);
		
		//load and read pages data
		website = new Pages();
		website.LoadPages(file);
		
		//program flag
		boolean flagConfig = true;
		
		//counters
		int ok = 0;
		int failed = 0;
		
		//run configurations
		for(String[] config : configs) {
			flagConfig = true;
			System.out.print("Configuration: ");
			current = website.getPage("main");	
			int i = 0;
			for(String s : config) {
				if(!searchAndDo(s))
				{
					flagConfig = false;
					break;
				}
				if(i == config.length-1)
					System.out.print(s);
				else
					System.out.print(s + ">");
				
				i += 1;
			}
			
			if(flagConfig) {
				System.out.println("\tOK");
				ok += 1;
			} else {
				System.out.println("\tFAILED");
				failed += 1;
			}
			
			Thread.sleep(30000);
			
			driver.get(homepage);
		}
		
		System.out.println("\nSuccessful Configurations: " + ok);
		System.out.println("Failed Configurations: "+ failed);
		
		if(failed>0) return false;
		else return true;
	}
	
	private List<String[]> readInputfile(String conf) {
		List<String[]> config = new ArrayList<String[]>();
		
		String line;
		BufferedReader bReader;
		try {
			bReader = new BufferedReader(new FileReader(conf));
			try {
				while((line = bReader.readLine()) != null) {
					config.add(line.split(">"));
				}
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		return config;
	}
	
	private boolean searchAndDo(String action) {
		PageElement element;
		if(current.isValid(action)) {
			if(previous.isEmpty()) {
				//System.out.println(current.pageElements.size());
				element = current.getElement(action);
				if(!element.equals(null)) {
					try {
						if(element.type.contentEquals("link") || element.type.contentEquals("checkbox")) {
							//driver.findElement(By.id(element.id)).click();
							wait.until(ExpectedConditions.elementToBeClickable(By.id(element.id))).click();
							if(!element.nextPage.contentEquals("null"))
								current = website.getPage(element.nextPage);
							return true;
						}
						else {
							previous = element.name;
						}
					} catch (Exception e) {
						System.out.println("\t[ERROR 101] CANNOT FIND: " + action);
					}
				}
			} else {
				element = current.getElement(previous);
				if(!element.equals(null)) {
					try {
					if(element.type.contentEquals("textbox")) {
						wait.until(ExpectedConditions.elementToBeClickable(By.id(element.id))).clear();
						wait.until(ExpectedConditions.elementToBeClickable(By.id(element.id))).sendKeys(action);
						//driver.findElement(By.id(element.id)).clear();
						//driver.findElement(By.id(element.id)).sendKeys(action);
						previous = "";
						return true;
					} else if(element.type.contentEquals("dropdown") || element.type.contentEquals("select")) {
						new Select(wait.until(ExpectedConditions.elementToBeClickable(By.id(element.id)))).selectByVisibleText(action);
						previous = "";
						return true;
					} else if(element.type.contentEquals("radio")) {
						element = current.getElement(previous, action);
						wait.until(ExpectedConditions.elementToBeClickable(By.id(element.id))).click();
						previous = "";
						//driver.findElement(By.id(element.id)).click();
						return true;
					}
					} catch (Exception e) {System.out.println("\t[ERROR 101] CANNOT FIND: " + action);}
				} 
			}
		} else System.out.println("\t[ERROR 100] INVALID INPUT: " + action);
	    return false;
	}
	
	public void dispose() {
		driver.close();
		driver.quit();
		System.out.println("Closing Selenium Node . . .  OK");
		System.out.println("Closing Selenium Hub Session . . .  OK");
	}
}
