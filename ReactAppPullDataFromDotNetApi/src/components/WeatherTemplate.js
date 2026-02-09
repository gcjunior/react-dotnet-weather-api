/* File : src/App.js*/

import { useFormik } from "formik";
import throttle from "lodash.throttle";
import { useCallback, useState } from "react";
import * as Yup from "yup";
import "../App.css";
import { getBackgroundGif } from "./constants";

const WeatherTemplate = () => {
  const [weatherData, setWeatherData] = useState(null);

  const validationSchema = Yup.object({
    cityInput: Yup.string().min(4, "City is required").required("Required"),
  });

  // Function to fetch weather data from the weatherapi.com API
  const fetchWeatherData = async (city) => {
    // Implement API call to fetch weather data
    // using a service of weatherapi.com
    // Replace the API_KEY with your actual API key
    const query = city
      .split(",")
      .map((part) => part.trim())
      .join("/");
    const API_ENDPOINT = `http://localhost:5000/api/weatherforecastwithopenweather/city/${query}`;

    try {
      // Fetch weather data from the API
      const response = await fetch(API_ENDPOINT);
      const data = await response.json();

      // Update the state with the fetched weather data
      setWeatherData(data);
    } catch (error) {
      // Handle errors in fetching weather data
      console.error("Error fetching weather data:", error);
    }
  };

  // Throttle function: only runs at most once per 500ms
  const throttledUpdateCity = useCallback(
    throttle((newCity) => {
      fetchWeatherData(newCity);
    }, 1000),
    [],
  );

  // Formik setup
  const formik = useFormik({
    initialValues: {
      cityInput: "",
    },
    validationSchema: validationSchema,
    onSubmit: (values) => {
      throttledUpdateCity(values.cityInput);
      formik.resetForm();
    },
  });

  const handleReset = (resetForm) => {
    if (window.confirm("Reset?")) {
      setWeatherData(null);
      resetForm();
    }
  };

  const backgroundGif = getBackgroundGif(weatherData?.weather?.[0]?.main);

  // JSX structure for the Weather App component
  return (
    <div
      className="w-80 p-8 rounded-2xl bg-gradient-to-b from-[#3a4a7a] to-[#f17e2d]
                 backdrop-blur-2xl shadow-lg text-white font-['Roboto'] select-none mx-auto
                 text-center"
      style={{
        backgroundRepeat: "no-repeat",
        backgroundSize: "100px 10ppx",
        backgroundPosition: "center 30%",
        backgroundImage: weatherData ? `url(${backgroundGif})` : "none",
      }}
    >
      {/* City Input Formik */}
      <form
        onSubmit={formik.handleSubmit}
        className="mb-6 flex gap-2 justify-center"
        aria-label="City selection form"
      >
        <div className="mb-6 flex gap-2 justify-center relative">
          <input
            type="text"
            name="cityInput"
            value={formik.values.cityInput}
            onChange={formik.handleChange}
            placeholder="Enter city"
            className="rounded-l-md px-3 py-1 text-black w-full focus:outline-none"
            aria-label="City name input"
          />
          <button
            type="submit"
            className="bg-white text-[#3a4a7a] font-semibold px-4 py-1 rounded-r-md hover:bg-gray-200 transition"
            aria-label="Submit city"
          >
            Check
          </button>
          <button
            type="button"
            onClick={() => handleReset(formik.resetForm)}
            className="bg-white text-[#3a4a7a] font-semibold px-4 py-1 rounded-r-md hover:bg-gray-200 transition"
            aria-label="Reset"
          >
            Reset
          </button>
          {/* Conditionally render error if the field is touched AND has an error */}
          {formik.touched.cityInput && formik.errors.cityInput ? (
            <div className="absolute left-0 top-8 text-red-500 text-shadow">
              {formik.errors.cityInput}
            </div>
          ) : null}
        </div>
      </form>
      {weatherData && (
        <>
          {/* Header */}
          <div className="mb-4 flex justify-center items-center gap-1 text-2xl font-light text-shadow-sm text-shadow-black">
            {weatherData?.name}{" "}
            <span className="font-bold text-xl opacity-70">➔</span>
          </div>

          {/* Current weather */}
          <div className="mb-4">
            <div className="text-6xl mb-[-0.5rem]">
              {weatherData?.weather?.icon}
            </div>
            <div className="text-5xl font-bold leading-none mb-2  text-shadow-sm text-shadow-black">
              {weatherData?.main?.temp}°
            </div>
            <div className="flex justify-center gap-8 font-light text-xl mb-2  text-shadow-sm text-shadow-black">
              <span>{weatherData?.main?.temp_min}°</span>
              <span>{weatherData?.main?.temp_max}°</span>
            </div>
            <div className="text-base font-light opacity-85  text-shadow-sm text-shadow-black">
              {weatherData?.weather.map((w) => w.description).join(", ")}
            </div>
          </div>
        </>
      )}

      <hr className="border-t border-white/30 my-4" />
    </div>
  );
};

export default WeatherTemplate;
