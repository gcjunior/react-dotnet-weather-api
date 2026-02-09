import * as Yup from "yup";

export const WeatherSchema = Yup.object().shape({
  cityInput: Yup.string()
    .min(1, "Too Short!")
    .max(50, "Too Long!")
    .required("Required"),
});
