interface ISubscriptionInfo {
	toJSON(): string;
}

class W3cSubscriptionInfo {

	private _p256dh: string;
	private _auth: string;
	private _endpoint: string;

	constructor(data: PushSubscription) {
		this._p256dh = btoa(
			String.fromCharCode.apply(null, new Uint8Array(data.getKey('p256dh')))
		);
		this._auth = btoa(
			String.fromCharCode.apply(null, new Uint8Array(data.getKey('auth')))
		);
		this._endpoint = data.endpoint;
	}

	public toJSON(): string {
		return JSON.stringify({
			endpoint: this._endpoint,
			publicKey: this._p256dh,
			auth: this._auth
		});
	}
}

class SafariSubscriptionInfo {
	private deviceToken: string;

	constructor(deviceToken: string) {
		this.deviceToken = deviceToken;
	}

	public toJSON(): string {
		return JSON.stringify({ deviceToken: this.deviceToken });
	}
}

export { ISubscriptionInfo, W3cSubscriptionInfo, SafariSubscriptionInfo }