//
// https://stackoverflow.com/questions/9847580/how-to-detect-safari-chrome-ie-firefox-and-opera-browser
//

enum BrowserType {
	Opera,
	Firefox,
	Safari,
	IE,
	Edge,
	Chrome,
	Blink
}

declare global {
	interface Window {
		opr: any;
		opera: any;
		HTMLElement: any;
		[index: string]: any;
	}

	interface Document {
		documentMode: any;
	}
}

declare abstract class opr {
	public static addons: any;
}

declare abstract class safari {
	public static pushNotification: any;
}

declare let InstallTrigger: any;

class Browser {
	// Opera 8.0+
	public static isOpera: boolean = (!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;

	// Firefox 1.0+
	public static isFirefox = typeof InstallTrigger !== 'undefined';

	// Safari 3.0+ "[object HTMLElementConstructor]" 
	public static isSafari = /constructor/i.test(window.HTMLElement) || (function (p) { return p.toString() === "[object SafariRemoteNotification]"; })(!window['safari'] || (typeof safari !== 'undefined' && safari.pushNotification));

	// Internet Explorer 6-11
	public static isIE = /*@cc_on!@*/false || !!document.documentMode;

	// Edge 20+
	public static isEdge = !Browser.isIE && !!window.StyleMedia;

	// Chrome 1+
	public static isChrome = !!window.chrome && !!window.chrome.webstore;

	// Blink engine detection
	public static isBlink = (Browser.isChrome || Browser.isOpera) && !!window.CSS;

	public static getBrowserType(): BrowserType {
		if (this.isOpera) return BrowserType.Opera;
		if (this.isFirefox) return BrowserType.Firefox;
		if (this.isSafari) return BrowserType.Safari;
		if (this.isIE) return BrowserType.IE;
		if (this.isEdge) return BrowserType.Edge;
		if (this.isChrome) return BrowserType.Chrome;
		if (this.isBlink) return BrowserType.Blink;

		return null;
	}
}

export { Browser, BrowserType };