package content.bodyPart.entity;

import org.springframework.util.Assert;

public class BodyPartNameDtoFactory {

    public BodyPartNameDto create(BodyPart bodyPart) {

        Assert.notNull(bodyPart, "bodyPart must not be null");

        return new BodyPartNameDto(
                bodyPart.getName()
        );
    }
}
