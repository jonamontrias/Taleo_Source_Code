package selenium.devops.data;

import java.util.ArrayList;
import java.util.List;

public class Page {
	public String name = "";
	public List<PageElement> pageElements = new ArrayList<PageElement>();
	
	public PageElement getElement(String action) {
		for(PageElement element : pageElements) {
			if(element.name.contentEquals(action))
				return element;
		}
		return null;
	}
	
	public PageElement getElement(String action, String val) {
		for(PageElement element : pageElements) {
			if(element.name.contentEquals(action))
				if(element.nextPage.contentEquals(val))
					return element;
		}
		return null;
	}
	
	public boolean isValid(String action) {
		for(PageElement element : pageElements) {
			if(element.name.contentEquals(action))
				return true;
		}
		return false;
	}
	
	public Page clone() {
		List<PageElement> newElements = new ArrayList<PageElement>();
		String newName = this.name;
		
		newElements.addAll(this.pageElements);
		Page newPage = new Page();
		
		newPage.name = newName;
		
		newPage.pageElements = newElements;
		
		return newPage;
	}
	
	public int count() {
		return pageElements.size();
	}
}