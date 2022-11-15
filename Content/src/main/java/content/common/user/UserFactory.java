package content.common.user;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.util.Assert;

import java.util.UUID;
import java.util.logging.Level;
import java.util.logging.Logger;

public class UserFactory {

    public User create(String userJson, UUID userId) {

        Assert.notNull(userJson, "userJson must not be null");

        ObjectMapper objectMapper = new ObjectMapper();

        try {

            JsonNode userNode = objectMapper.readTree(userJson);

            Logger.getGlobal().log(Level.WARNING, userNode.asText());

            return new User(
                    userId,
                    userNode.get("firstName").asText(),
                    userNode.get("lastName").asText(),
                    userNode.get("avatarUrl").asText()
            );
        }
        catch (JsonProcessingException e) {

            throw new UserParseException("User structure has changed");
        }
    }
}
