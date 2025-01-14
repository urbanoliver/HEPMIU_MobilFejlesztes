using WeaponMobileApp.BusinessModel;
using System.Text.Json;
using System.Diagnostics;
using System.Text;

namespace WeaponMobileApp.Services
{
    public class WeaponService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        private string windowsURI = "https://localhost:44331/api/Weapons/";
        private string androidURI = "https://10.0.2.2:44331/api/Weapons/";

        public WeaponService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<List<Weapon>> GetWeaponsAsync()
        {
            var weapons = new List<Weapon>();
            //Uri uri = new Uri(androidURI);
            Uri uri = new Uri(windowsURI);
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    weapons = JsonSerializer.Deserialize<List<Weapon>>(content, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return weapons;
        }

        public async Task<Weapon> GetWeaponByIdAsync(int Id)
        {
            try
            {
                Uri uri = new Uri($"{windowsURI}{Id}");

                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<Weapon>(json, _serializerOptions);
                }
                else
                {
                    Debug.WriteLine($"Failed to fetch weapon with ID {Id}. Status Code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching weapon by ID: {ex.Message}");
                return null;
            }
        }


        public async Task SaveWeaponAsync(UpdateWeaponDTO weapon, List<Weapon> existingWeapons, int? Id = null)
        {
            if (weapon == null)
            {
                throw new ArgumentNullException(nameof(weapon), "Weapon data cannot be null.");
            }

            Uri uri;
            string json = JsonSerializer.Serialize(weapon, _serializerOptions);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                bool isExistingWeapon = Id.HasValue && existingWeapons.Any(w => w.Id == Id.Value);

                if (isExistingWeapon)
                {
                    uri = new Uri($"{windowsURI}{Id}");
                    Debug.WriteLine($"Updating existing weapon. Request URL: {uri}, Payload: {json}");

                    HttpResponseMessage response = await _httpClient.PatchAsync(uri, content);
                    string responseContent = await response.Content.ReadAsStringAsync();

                    Debug.WriteLine($"Response Status Code: {response.StatusCode}");
                    Debug.WriteLine($"Response Content: {responseContent}");

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"Failed to update weapon. Status Code: {response.StatusCode}, Response: {responseContent}");
                    }

                    Debug.WriteLine("Weapon successfully updated via PATCH.");
                }
                else
                {
                    uri = new Uri(windowsURI);
                    Debug.WriteLine($"Creating new weapon. Request URL: {uri}, Payload: {json}");

                    HttpResponseMessage response = await _httpClient.PostAsync(uri, content);
                    string responseContent = await response.Content.ReadAsStringAsync();

                    Debug.WriteLine($"Response Status Code: {response.StatusCode}");
                    Debug.WriteLine($"Response Content: {responseContent}");

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"Failed to create weapon. Status Code: {response.StatusCode}, Response: {responseContent}");
                    }

                    Debug.WriteLine("Weapon successfully created via POST.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occurred while saving weapon: {ex.Message}");
                throw;
            }
        }



        public async Task DeleteWeaponAsync(int Id)
        {
            Uri uri = new Uri($"{windowsURI}{Id}");

            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\tWeapon successfully deleted.");
                }
                else
                {
                    Debug.WriteLine(@"\tFailed to delete weapon. Status Code: {0}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR: {0}", ex.Message);
            }
        }

    }
}
