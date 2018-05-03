export class StringUtils {
	public static urlB64ToUint8Array(base64String: string): Uint8Array {
		let padding = '='.repeat((4 - base64String.length % 4) % 4);
		var base64 = (base64String + padding)
			.replace(/\-/g, '+')
			.replace(/_/g, '/');

		var rawData = window.atob(base64);
		var outputArray = new Uint8Array(rawData.length);

		for (var i = 0; i < rawData.length; ++i) {
			outputArray[i] = rawData.charCodeAt(i);
		}
		return outputArray;
	}
}