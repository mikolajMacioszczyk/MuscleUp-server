package content.bodyPart.entity;

import org.springframework.util.Assert;

public class BodyPartDtoFactory {

    public BodyPartDto create(BodyPart bodyPart) {

        Assert.notNull(bodyPart, "bodyPart must not be null");

        return new BodyPartDto(
                bodyPart.getId(),
                bodyPart.getName()
        );
    }
}
