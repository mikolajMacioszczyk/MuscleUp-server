package content.bodyPart.entity;

import content.bodyPart.controller.form.BodyPartForm;
import org.springframework.util.Assert;

public class BodyPartFactory {

    public BodyPart create(BodyPartForm bodyPartForm) {

        Assert.notNull(bodyPartForm, "bodyPartForm must not be null");

        return new BodyPart(
                bodyPartForm.name()
        );
    }
}
