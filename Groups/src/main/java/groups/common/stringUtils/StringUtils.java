package groups.common.stringUtils;

import static java.util.Objects.isNull;

public class StringUtils {

    private static final String EMPTY_STRING = "";


    public static boolean isNullOrEmpty(String target) {

        return isNull(target) || target.isEmpty();
    }

    public static String concatenate(String... args) {

        StringBuilder stringBuilder = new StringBuilder();
        String correctArg;

        for(String arg : args) {

            correctArg = isNull(arg)? EMPTY_STRING : arg;
            stringBuilder.append(correctArg);
        }

        return stringBuilder.toString();
    }
}
