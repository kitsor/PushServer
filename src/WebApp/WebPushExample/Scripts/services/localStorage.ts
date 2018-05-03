interface IData {
	[key: string]: any;
}

class LocalStorage {

	constructor(private namespace: string = "wpsManager") {

	}

	public setItem(key: string, item: any) {
		let items = this.getItems();
		items[key] = item;
		this.setItems(items);
	}

	public getItem(key: string): any {
		let items = this.getItems();
		return key in items ? items[key] : null;
	}

	public hasKey(key: string): boolean {
		let items = this.getItems();
		return key in items;
	}

	public removeItem(key: string): void {
		let items = this.getItems();
		delete items[key];
		this.setItems(items);
	}

	private getItems(): IData {
		let items = window.localStorage.getItem(this.namespace);
		return items !== null ? JSON.parse(localStorage.getItem(this.namespace)) : {};
	}

	private setItems(items: IData): void {
		window.localStorage.setItem(this.namespace, JSON.stringify(items));
	}
}

export { LocalStorage }