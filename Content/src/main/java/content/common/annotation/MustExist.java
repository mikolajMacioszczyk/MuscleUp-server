package content.common.annotation;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

@Target({ ElementType.METHOD, ElementType.CONSTRUCTOR, ElementType.FIELD, ElementType.TYPE})
@Retention(RetentionPolicy.SOURCE)
public @interface MustExist {

    Reason[] reason() default Reason.OTHER;

    String description() default "";

    enum Reason {

        HIBERNATE,
        OTHER
    }
}
