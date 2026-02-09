export const icons = {
  thunderstorm: "⛈️",
  rain: "🌧️",
  cloud: "☁️",
  sun: "☀️",
};

export const getBackgroundGif = (weatherMain) => {
  const main = {
    'Snow': './assets/snow.gif',
    'Rain': './assets/rain-weather.gif',
    'Drizzle': './assets/rain-drops.gif',
    'Thunderstorm': './assets/thunderstorm.gif',
    'Clear': './assets/clear.gif',
    'Clouds': './assets/clouds.gif',
    'Atmosphere': './assets/atmosphere.gif',
  } 
  return main[weatherMain] || weatherMain || '';
};