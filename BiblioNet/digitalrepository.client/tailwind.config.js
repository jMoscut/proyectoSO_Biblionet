const { heroui } = require("@heroui/theme");

/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./index.html",
    "./src/**/*.{html,js,ts,jsx,tsx,css}",
    "./node_modules/@heroui/theme/dist/components/(button|calendar|card|checkbox|date-picker|drawer|form|image|input|modal|navbar|select|toggle|toast|popover|ripple|spinner|date-input|listbox|divider|scroll-shadow).js",
  ],
  theme: {
    extend: {},
  },
  darkMode: "class",
  plugins: [heroui()],
};
