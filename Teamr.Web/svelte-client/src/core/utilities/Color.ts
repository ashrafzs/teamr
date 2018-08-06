// tslint:disable:no-bitwise

export class Color {
	public static stringToColor(str: string, percent?: number): string {
		let hash = 0;
		for (let i = 0; i < str.length; i++) {
			hash = str.charCodeAt(i) + ((hash << 5) - hash);
		}
		let colour = "#";
		for (let i = 0; i < 3; i++) {
			const value = (hash >> (i * 8)) & 0xFF;
			colour += ("00" + value.toString(16)).substr(-2);
		}
		return colour;
		// percent = percent == null ? 1 : percent;

		// if (str === undefined || str === null) {
		// 	return Color.shadeColor2("#000", percent);
		// }

		// let hash = 0;
		// let colour = "#";

		// for (let i = 0; i < str.length; ++i) {
		// 	hash = str.charCodeAt(i++) + ((hash << 5) - hash);
		// }

		// for (let j = 0; j < 3; ++j) {
		// 	colour += ("00" + ((hash >> j++ * 8) & 0xFF).toString(16)).slice(-2);
		// }

		// return Color.shadeColor2(colour, percent);
	}

	public static shadeColor2(color: string, percent: number): string {
		const f = parseInt(color.slice(1), 16);
		const t = percent < 0 ? 0 : 255;
		const p = percent < 0 ? percent * -1 : percent;
		const R = f >> 16;
		const G = f >> 8 & 0x00F;
		const B = f & 0x0000FF;

		return "#" + (0x1000000 + (Math.round((t - R) * p) + R) * 0x10000 + (Math.round((t - G) * p) + G) * 0x100 + (Math.round((t - B) * p) + B)).toString(16).slice(1);
	}

	public static contrastColor(color: string): string {
		const r = parseInt(color.substr(1, 2), 16);
		const g = parseInt(color.substr(3, 2), 16);
		const b = parseInt(color.substr(5, 2), 16);

		// Counting the perceptive luminance - human eye favors green color...
		const a = 1 - (0.299 * r + 0.587 * g + 0.114 * b) / 255;

		if (a < 0.5) {
			return "#000";
		} else {
			return "#fff";
		}
	}
}
