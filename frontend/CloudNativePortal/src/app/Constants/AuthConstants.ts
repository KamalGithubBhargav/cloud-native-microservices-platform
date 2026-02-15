import { globalConstants } from "./globalConstants";

export const authConstants = {
  loginUrl: `${globalConstants.BaseUrl}/Auth/Login`,
  refreshUrl: `${globalConstants.BaseUrl}/Auth/refresh`
}

export const authMessageConstants = {
  successMessage: "User logged in successfully",
  error401Message: "Invalid username or password",
  errorServerNotResponding: "Server not reachable. Check your network.",
}
