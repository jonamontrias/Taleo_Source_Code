package selenium.devops.data;
import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

import selenium.devops.sel_automate.*;

public class Pages {
		
	public Map<String, Page> pages;
	
	public void LoadPages(String file) { 
		try {
			pages = new HashMap<String, Page>();
			BufferedReader BReader = new BufferedReader(new FileReader(file));
			String s = "";
			try {
				String[] element;
				List<PageElement> pageElements = new ArrayList<PageElement>();
				PageElement _pageElement = new PageElement();
				Page _page = new Page();
				
				while((s = BReader.readLine()) != null) {
					if(s.charAt(0) == '/' && pageElements.size() == 0) {
						_page.name = s.substring(2);
					}
					else if(s.charAt(0) == '/' && pageElements.size() > 0) {
						_page.pageElements = pageElements;
						pages.put(_page.name, _page.clone());
						_page = new Page();
						pageElements.clear();
						
						_page.name = s.substring(2);
					} else {
						_pageElement = new PageElement();
						_pageElement.name = s.split(" \\| ")[0];
						_pageElement.id = s.split(" \\| ")[1];
						_pageElement.type = s.split(" \\| ")[2];
						_pageElement.nextPage = s.split(" \\| ")[3];
						pageElements.add(_pageElement);
					}
				}
				
				_page.pageElements = pageElements;
				pages.put(_page.name, _page.clone());

			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	private void printMap(Map mp) {
	    Iterator it = mp.entrySet().iterator();
	    while (it.hasNext()) {
	        Map.Entry pair = (Map.Entry)it.next();
	        System.out.println(pair.getKey() + " = " + pair.getValue());
	        it.remove(); // avoids a ConcurrentModificationException
	    }
	}
	
	public Page getPage(String key) {
		Iterator it = pages.entrySet().iterator();
		while (it.hasNext()) {
			Map.Entry<String, Page> pair = (Map.Entry<String, Page>) it.next();
			//System.out.println(pair.getKey() + " = " + pair.getValue().pageElements.size());
			if(pair.getKey().contentEquals(key)) {
				
				return pair.getValue();
			}
		}
		return null;
	}
	
	private List<String> readFile(String file) {
		return null;
	}
}