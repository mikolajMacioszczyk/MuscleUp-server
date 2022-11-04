package groups.groupTrainer.trainer;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.util.Assert;

import java.util.UUID;

public class TrainerFactory {

    public Trainer create(String trainerJson) {

        Assert.notNull(trainerJson, "trainerJson must not be null");

        ObjectMapper objectMapper = new ObjectMapper();

        try {

            JsonNode trainerNode = objectMapper.readTree(trainerJson);

            return new Trainer(
                    UUID.fromString(trainerNode.get("userId").asText()),
                    trainerNode.get("firstName").asText(),
                    trainerNode.get("lastName").asText(),
                    trainerNode.get("avatarUrl").asText()
            );
        }
        catch (JsonProcessingException e) {

            throw new TrainerParserException("Trainer structure has changed");
        }
    }
}
