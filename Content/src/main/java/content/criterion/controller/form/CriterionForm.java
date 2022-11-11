package content.criterion.controller.form;

import org.springframework.lang.Nullable;

public record CriterionForm(String name, @Nullable String unit) {
}
