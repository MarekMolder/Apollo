import axios from "axios";

export class LocalizationService {
  private baseUrl = "http://localhost:5294/api/v1/localization";

  async getTranslations(language: string, resource: string): Promise<Record<string, string>> {
    try {
      const response = await axios.get(`${this.baseUrl}/get`, {
        params: {
          language,
          resource
        }
      });
      return response.data as Record<string, string>;
    } catch (error) {
      console.error("Failed to fetch translations", error);
      throw error;
    }
  }
}
