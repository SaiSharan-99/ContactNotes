using ContactNotesAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json;
using System.Text;

namespace ContactNotesAPI.Helpers
{
    public class CreateNoteDtoInputFormatter : TextInputFormatter
    {
        public CreateNoteDtoInputFormatter()
        {
            SupportedMediaTypes.Add("application/json");
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanReadType(Type type)
        {
            return type == typeof(CreateNoteDto);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            var request = context.HttpContext.Request;

            using var reader = new StreamReader(request.Body, encoding);
            var body = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(body))
                return await InputFormatterResult.SuccessAsync(new CreateNoteDto());

            Dictionary<string, JsonElement> json;
            try
            {
                json = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException)
            {
                return await InputFormatterResult.SuccessAsync(new CreateNoteDto());
            }

            var result = new CreateNoteDto();

            if (json.TryGetValue("contactId", out var contactIdElem))
            {
                if (Guid.TryParse(contactIdElem.ToString(), out var contactId))
                {
                    result.ContactId = contactId;
                }
            }

            var possibleKeys = new[] { "body", "note", "note_body", "noteBody", "note_text", "text" };

            foreach (var key in possibleKeys)
            {
                if (json.TryGetValue(key, out var bodyElem))
                {
                    result.Body = bodyElem.GetString();
                    break;
                }
            }

            return await InputFormatterResult.SuccessAsync(result);
        }
    }
}
